"""Private-chat Telegram control and durable event delivery (long polling)."""
import asyncio
import json
import logging
import os
import secrets
import time
from pathlib import Path
import httpx
from .config import Settings
from .control import Control
from .tbank import TBankClient
from .models import encode
from .journal import Journal
from .runtime import ProcessLock
from .vpn import save_config, start as start_vpn, stop as stop_vpn, local_http_proxy
from .instruments import fetch_rub_instruments
from .api_facade import BrokerFacade
from .learning import LearningStore
from .token_budget import count_tokens
from .telegram_views import report as transaction_report, pnl_report, LEARNING, event_text, Reply, ru, stamp

INSTRUMENT_LIST_PATH = Path(__file__).resolve().parents[1] / 'instrumentlist.json'


def _load_instrument_list():
    if not INSTRUMENT_LIST_PATH.exists():
        return []
    try:
        data = json.loads(INSTRUMENT_LIST_PATH.read_text(encoding='utf-8-sig'))
    except (OSError, json.JSONDecodeError):
        return []
    return data.get('instruments', []) if isinstance(data, dict) else data if isinstance(data, list) else []

HELP = '''Команды бота:
/status — состояние бота, режим и пауза
/balance — деньги на счёте и доступный остаток
/portfolio — позиции, лоты и прибыль/убыток
/orders — активные заявки Sandbox
/transactions [all|profit|loss|zero|unknown] [страница] — вся сохранённая история
/pnl — общие итоги и результат по активам
/abort — отменить ввод или подтверждение
/errors — последние ошибки
/set max_drawdown_rub ЧИСЛО — предел просадки, 1–1000000 ₽
/set max_exposure_rub ЧИСЛО — предел вложений, 1–10000000 ₽
/instruments — включённые инструменты: UID/FIGI и рублёвый или нет
/learning — статистика накопленного опыта
/api — меню возможностей T-Invest API
/api account|info|tariff|margin|withdraw|operations|market|candles|instrument|search|dividends|coupons|schedule|favorites|countries|stoporders ПАРАМЕТРЫ
/search ТЕКСТ — найти доступный инструмент
/add UID — добавить инструмент выключенным
/enable UID /disable UID — включить/выключить инструмент
/instrument НОМЕР — выбрать инструмент по номеру из меню /instruments
/sync_instruments — обновить каталог только поддерживаемых инструментов
/list_instruments — список из instrumentlist.json
/enable_list — импортировать файл и включить доступные рублёвые акции (подтверждение)
/set interval ЧИСЛО — интервал торгового цикла, 30–3600 сек
/set notify_interval ЧИСЛО — отчёт по транзакциям, 60–86400 сек
/set max_order_lots ЧИСЛО — максимум лотов заявки (1 и выше)
/tokens ТЕКСТ — проверить расчёт токенов через API Duck
/manual — полный мануал внутри бота
/settings — текущие настройки
/pause /resume — пауза или возобновление (подтверждение)
/mode observe|sandbox — режим, production запрещён
/notify all|important|off — фильтр уведомлений
/cancel ORDER_ID — отмена заявки (подтверждение)
/id — показать Telegram ID
/friend_add ID — дать другу управление (владелец, подтверждение)
/friend_remove ID — отозвать доступ (владелец)
/friends — список друзей (владелец)
Друг: просмотр счёта, инструменты, пауза/запуск, отмена заявок.
Настройки, режим, прокси и выдача доступа — только владельцу.
/vpn — статус прокси; /vpn_json — настроить прокси
/start /help — эта справка'''

# Главное меню: безопасные команды доступны одним нажатием. Команды с параметрами
# (поиск, добавление и настройки) остаются в /help, чтобы случайно не изменить торговлю.
KEYBOARD = {
    'keyboard': [
        ['📊 Статус', '💰 Баланс'],
        ['📈 Портфель', '🧾 Все операции'],
        ['✅ Прибыльные', '❌ Убыточные'],
        ['🧰 Инструменты', '📚 Список из JSON'],
        ['🔄 Синхронизация', '📋 Заявки'],
        ['🧠 Обучение', '🔌 API'],
        ['⚠️ Ошибки', '❓ Помощь'],
        ['🔎 Поиск', '➕ Добавить'],
        ['⚙️ Настройки', '⏸ Пауза'],
        ['▶️ Возобновить', '❓ Помощь'],
    ],
    'resize_keyboard': True,
    'is_persistent': True,
}


def _field(obj, *names, default=None):
    for name in names:
        if isinstance(obj, dict) and name in obj:
            return obj[name]
        if hasattr(obj, name):
            return getattr(obj, name)
    return default


def _num(value):
    if isinstance(value, dict) or hasattr(value, 'units'):
        return float(_field(value, 'units', default=0)) + float(_field(value, 'nano', default=0)) / 1_000_000_000
    try:
        return float(value)
    except (TypeError, ValueError):
        return 0


def _money(value):
    if value is None:
        return 'нет данных'
    currency = str(_field(value, 'currency', default='rub')).upper()
    return f'{_num(value):,.2f}'.replace(',', ' ') + (' ₽' if currency == 'RUB' else ' ' + currency)


def _money_group(value):
    if isinstance(value, (list, tuple)):
        return ', '.join(_money(x) for x in value) or 'нет данных'
    return _money(value)


def _balance_text(portfolio, positions=None):
    lines = ['💰 Счёт Sandbox', 'Стоимость портфеля: ' + _money_group(_field(portfolio, 'total_amount_portfolio')),
             'Валютная часть: ' + _money_group(_field(portfolio, 'total_amount_currencies'))]
    if positions is not None:
        blocked = {x.currency.lower(): _num(x) for x in positions.blocked}
        for money in positions.money:
            cur = money.currency.lower()
            held = blocked.pop(cur, 0)
            lines.append(f'{cur.upper()}: всего {_money(money)}, заблокировано {held:,.2f}, доступно {_num(money)-held:,.2f}')
        for cur, held in blocked.items():
            lines.append(f'{cur.upper()}: заблокировано {held:,.2f}; остаток не получен')
        if not positions.money:
            lines.append('Денежных остатков нет.')
    pnl = _field(portfolio, 'expected_yield')
    lines.append('Доходность портфеля: ' + (f'{_num(pnl):+.2f} %' if pnl is not None else 'нет данных'))
    return '\n'.join(lines)


def _portfolio_text(portfolio):
    lines = [_balance_text(portfolio)]
    positions = _field(portfolio, 'positions', default=[]) or []
    lines.append(f'\n📈 Позиции: {len(positions)}')
    for p in positions:
        figi = _field(p, 'figi', 'instrument_uid', default='?')
        lots = _field(p, 'quantity_lots')
        quantity = _num(_field(p, 'quantity'))
        pnl = _field(p, 'expected_yield')
        avg = _field(p, 'average_position_price')
        count = f'{_num(lots):g} лот(ов)' if lots is not None else 'лоты: нет данных'
        profit = 'прибыль' if _num(pnl) >= 0 else 'убыток'
        currency = _field(avg, 'currency', default='rub')
        amount = _money({'units': _num(pnl), 'currency': currency}) if pnl is not None else 'нет данных'
        lines.append(f'{figi}: {count}, {quantity:g} шт., средняя {_money(avg)}, {profit} {amount}')
    return '\n'.join(lines)


def _instrument_line(item):
    currency = str(item.get('currency', '')).lower()
    label = 'рублёвый' if currency == 'rub' else 'не рублёвый' if currency else 'валюта не указана'
    return f"{item.get('figi') or 'FIGI не указан'} — {label}"


class Commands:
    def __init__(self, s, control, users):
        self.s, self.c, self.users = s, control, set(users)
        self.pending = {}
        self.dialog = {}
        self.history = {}
        self.owners = set(users)

    def allowed_users(self):
        return self.owners | set(self.c.get('telegram_friends', []))

    async def handle(self, message):
        user = message.get('from', {}).get('id')
        chat = message.get('chat', {})
        if chat.get('type') != 'private' or chat.get('id') != user:
            return None
        if user not in self.allowed_users():
            return f'Ваш Telegram ID: {user}. Попросите владельца выполнить /friend_add {user}.' if message.get('text', '').split('@')[0] in {'/id', '/start'} else None
        # Never execute old queued commands after an outage.
        if abs(time.time() - message.get('date', 0)) > 120:
            return 'Команда устарела; отправьте повторно.'
        text = message.get('text', '').strip()
        aliases = {
            '📊 Статус':'/status', '💰 Баланс':'/balance', '📈 Портфель':'/portfolio',
            '🧾 Все операции':'/transactions', '🧾 Транзакции':'/transactions',
            '✅ Прибыльные':'/transactions profit', '❌ Убыточные':'/transactions loss',
            '⚪ Безубыточные':'/transactions zero', '◻️ Без результата':'/transactions unknown',
            '📊 Итоги':'/pnl', '🧰 Инструменты':'/instruments', '📚 Список из JSON':'/list_instruments',
            '🔄 Синхронизация':'/sync_instruments', '📋 Заявки':'/orders', '🧠 Обучение':'/learning','🔌 API':'/api',
            '👤 API: счета':'/api account','ℹ️ API: профиль':'/api info','💳 API: тариф':'/api tariff',
            '🛡 API: маржа':'/api margin','💸 API: вывод':'/api withdraw','🧾 API: операции':'/api operations',
            '📉 API: рынок':'/api market','🕯 API: свечи':'/api candles','📄 API: инструмент':'/api instrument',
            '🔍 API: поиск':'/api search','💰 API: дивиденды':'/api dividends','🧷 API: купоны':'/api coupons',
            '🗓 API: расписание':'/api schedule','⭐ API: избранное':'/api favorites','🌍 API: страны':'/api countries',
            '⛔ API: стоп-заявки':'/api stoporders',
            '⚠️ Ошибки':'/errors', '🔎 Поиск':'/search', '➕ Добавить':'/add',
            '🟢 Включить актив':'/enable', '🔴 Выключить актив':'/disable', '📥 Включить JSON':'/enable_list',
            '⏸ Пауза':'/pause', '▶️ Возобновить':'/resume', '⚙️ Настройки':'/settings',
            '❓ Помощь':'/help', '🏠 Меню':'/start', '👥 Друзья':'/friends', '🪪 Мой ID':'/id',
            '👤 Дать доступ':'/friend_add', '🚫 Отозвать доступ':'/friend_remove',
            '🌐 Прокси':'/vpn', '📝 JSON прокси':'/vpn_json', '🛑 Отменить заявку':'/cancel',
            '👁 Наблюдение':'/mode observe', '🧪 Песочница':'/mode sandbox',
            '🔔 Все события':'/notify all', '🔔 Важные':'/notify important', '🔕 Без уведомлений':'/notify off',
        }
        if text in {'🏠 Меню', '↩️ Отмена', '/abort', '/start'}:
            self.dialog.pop(user, None)
            self.pending.pop(user, None)
            getattr(self, 'vpn_pending', set()).discard(user)
            if text in {'↩️ Отмена','/abort'}:
                return Reply('Действие отменено. Настройки не изменены.', KEYBOARD['keyboard'])
        if text == '✅ Подтвердить':
            pending = self.pending.get(user)
            text = '/confirm ' + (pending[0] if pending else 'expired')
        if text in {'⬅️ Ранее','➡️ Далее'}:
            mode, page = self.history.get(user, ('all',1))
            text = f'/transactions {mode} {max(1,page + (1 if text == "➡️ Далее" else -1))}'
        prompts = {
            '/search': 'Введите название или тикер актива, например Сбербанк.',
            '/add': 'Введите UID или FIGI инструмента. Он будет добавлен выключенным.',
            '/enable': 'Введите UID/FIGI из списка инструментов. Включение потребует подтверждения.',
            '/disable': 'Введите UID/FIGI из списка инструментов. Позиции не закрываются.',
            '/cancel': 'Введите ID активной заявки из раздела «Заявки». Отмена требует подтверждения.',
            '/friend_add': 'Введите числовой Telegram ID друга. Он получит управление ботом.',
            '/friend_remove': 'Введите числовой Telegram ID друга для отзыва доступа.',
            '/set interval': 'Введите интервал анализа: 30–3600 секунд.',
            '/set notify_interval': 'Введите интервал отчётов: 60–86400 секунд.',
            '/set max_order_lots': 'Введите максимум лотов заявки: целое число от 1 и выше.',
            '/set max_drawdown_rub': 'Введите предел просадки: 1–1000000 рублей.',
            '/set max_exposure_rub': 'Введите предел вложений: 1–10000000 рублей.',
        }
        aliases.update({'⏱ Цикл':'/set interval', '⏲ Отчёты':'/set notify_interval',
                        '📦 Лимит лотов':'/set max_order_lots', '🛡 Просадка':'/set max_drawdown_rub',
                        '💼 Лимит вложений':'/set max_exposure_rub'})
        original = text
        text = aliases.get(text,text)
        if text in prompts:
            if text.startswith(('/set','/friend')) and user not in self.owners:
                return 'Эта команда доступна только владельцу.'
            self.dialog[user] = (text, time.monotonic()+180)
            return Reply(prompts[text], [['↩️ Отмена','🏠 Меню']])
        if user in self.dialog and not text.startswith('/') and original not in aliases:
            command, expires = self.dialog.pop(user)
            if time.monotonic() > expires:
                return 'Время ввода истекло. Нажмите нужную кнопку ещё раз.'
            text = command + ' ' + text
        elif text.startswith('/'):
            self.dialog.pop(user,None)
        if user in self.owners and user in getattr(self, 'vpn_pending', set()) and text.startswith('{'):
            self.vpn_pending.discard(user)
            try:
                path = save_config(json.loads(text))
                return f'VPN-конфигурация сохранена: {path}. Перезапустите START_TELEGRAM.bat.'
            except (json.JSONDecodeError, ValueError) as exc:
                return f'Некорректная VPN-конфигурация: {exc}'
        # В меню /instruments выбор выполняется просто номером.
        if text.isdigit() and text in self.c.get('instrument_menu', {}):
            text = '/instrument ' + text
        args = text.split()
        if not args:
            return HELP
        cmd = args[0].split('@')[0].lower()
        args = args[1:]
        try:
            if cmd in {'/vpn_json', '/vpn', '/friend_add', '/friend_remove', '/friends', '/set', '/mode', '/notify'} and user not in self.owners:
                return 'Эта команда доступна только владельцу.'
            if cmd == '/vpn_json':
                raw = text[len(args[0]) + 1:].strip() if False else text.split(' ', 1)[1].strip() if ' ' in text else ''
                if not raw:
                    self.vpn_pending = getattr(self, 'vpn_pending', set())
                    self.vpn_pending.add(user)
                    return 'Режим приёма включён. Отправьте следующим сообщением полный JSON одним сообщением.'
                try:
                    path = save_config(json.loads(raw))
                    return f'VPN-конфигурация сохранена: {path}. Перезапустите START_TELEGRAM.bat.'
                except (json.JSONDecodeError, ValueError) as exc:
                    return f'Некорректная VPN-конфигурация: {exc}'
            if cmd == '/confirm':
                pending = self.pending.pop(user, None)
                if not pending or len(args) != 1 or not secrets.compare_digest(args[0], pending[0]) or time.monotonic() > pending[1]:
                    return 'Неверное или просроченное подтверждение.'
                return await self.execute(pending[2], pending[3], user=user)
            if cmd in {'/resume','/enable','/enable_list','/set','/cancel','/friend_add'} or (cmd == '/mode' and args == ['sandbox']):
                code = secrets.token_hex(4)
                self.pending[user] = (code, time.monotonic()+60, cmd, args)
                return Reply(f'Подтвердите действие: {cmd} {" ".join(args)}\n/confirm {code} (60 секунд).\nВключение актива и возобновление могут разрешить новые заявки в песочнице.', [['✅ Подтвердить', '↩️ Отмена']])
            return await self.execute(cmd, args, user=user)
        except (ValueError, IndexError):
            return '⚠️ Некорректные параметры. Проверьте диапазон и формат в /help или повторно выберите кнопку. Изменения не подтверждены.'
        except Exception as exc:
            logging.warning('TELEGRAM_COMMAND_FAILED type=%s', type(exc).__name__)
            return 'Операция не подтверждена. Проверьте /status и /orders; не повторяйте отмену вслепую.'

    async def execute(self, cmd, args, user=None):
        if user is not None and user not in self.allowed_users():
            return 'Доступ отозван.'
        if cmd in {'/vpn_json', '/vpn', '/friend_add', '/friend_remove', '/friends', '/set', '/mode', '/notify'} and user not in self.owners:
            return 'Эта команда доступна только владельцу.'
        if cmd in {'/friend_add', '/friend_remove'} and len(args) == 1:
            friend = int(args[0])
            if friend <= 0 or friend in self.owners:
                return 'Укажите положительный ID друга, не владельца.'
            friends = set(self.c.get('telegram_friends', []))
            if cmd == '/friend_add':
                friends.add(friend)
            else:
                friends.discard(friend)
                self.pending.pop(friend, None)
            self.c.put('telegram_friends', sorted(friends))
            return 'Доступ друга ' + ('разрешён. Друг должен открыть бота и нажать /start.' if cmd == '/friend_add' else 'отозван.')
        if cmd == '/friends' and not args:
            return Reply('👥 Друзья: ' + (', '.join(map(str, self.c.get('telegram_friends', []))) or 'нет'), [['👤 Дать доступ','🚫 Отозвать доступ'], ['🏠 Меню']])
        if cmd in {'/help', '/start'}:
            return Reply(HELP, KEYBOARD['keyboard'])
        if cmd in {'/list_instruments', '/enable_list'} and not args:
            source = _load_instrument_list()
            if not source:
                return 'instrumentlist.json не найден или пуст.'
            rows = [x for x in source if isinstance(x, dict) and (x.get('uid') or x.get('figi'))]
            if cmd == '/list_instruments':
                return '\n'.join(_instrument_line(x) for x in rows) or 'Список пуст.'
            current = self.c.instruments(self.s)
            catalog = self.c.get('instrument_catalog', {})
            enabled = unavailable = failed = limited = 0
            async with TBankClient(self.s) as broker:
                for row in rows:
                    key = row.get('uid') or row['figi']
                    catalog[key] = row
                    current.setdefault(key, False)
                    try:
                        item = await broker.instrument(key)
                    except Exception:
                        failed += 1
                        continue
                    catalog[key] = {'figi': item.figi, 'currency': item.currency}
                    # The trading strategy supports only RUB shares.
                    if item.instrument_type != 'share' or item.currency.lower() != 'rub' or not item.buy_available_flag or not item.sell_available_flag:
                        unavailable += 1
                        continue
                    current[key] = True
                    enabled += 1
            # Не поддерживаемые записи не сохраняем даже выключенными.
            current = {uid: flag for uid, flag in current.items()
                       if uid in catalog and uid not in {
                           key for key in current if key not in {
                               (row.get('uid') or row.get('figi')) for row in rows
                           }
                       }}
            self.c.put('instrument_catalog', catalog)
            self.c.put('instruments', current)
            return (f'В каталоге из файла: {len(rows)}. Включено для торговли: {enabled}. '
                    f'Не поддерживаются стратегией/неторгуемые: {unavailable}. Ошибки API: {failed}. '
                    'Торговля поддерживает только рублёвые акции; индексы и сырьевые индикаторы не включаются. /instruments')
        if cmd == '/sync_instruments' and not args:
            items = await fetch_rub_instruments(self.s)
            current = self.c.instruments(self.s)
            # Каталог является источником истины: неторгуемые записи удаляются.
            supported_uids = {item.uid for item in items if getattr(item, 'uid', None)}
            updated = {uid: current.get(uid, False) for uid in supported_uids}
            self.c.put('instruments', updated)
            self.c.put('instrument_catalog', {
                item.uid: {'figi': getattr(item, 'figi', None), 'ticker': getattr(item, 'ticker', ''),
                           'name': getattr(item, 'name', ''), 'currency': getattr(item, 'currency', 'rub')}
                for item in items if getattr(item, 'uid', None)
            })
            return (f'Каталог обновлён: {len(items)} поддерживаемых инструментов. '
                    f'Удалено неподдерживаемых/устаревших: {max(0, len(current) - len(updated))}. '
                    'Откройте /instruments и выберите номер.')
        if cmd == '/transactions':
            mode = args[0] if args and not args[0].isdigit() else 'all'
            page = int(args[-1]) if args and args[-1].isdigit() else 1
            text, page, pages = transaction_report(self.s.journal_path, mode, page)
            self.history[user] = (mode, page)
            return Reply(text, [['⬅️ Ранее', '➡️ Далее'], ['🧾 Все операции', '✅ Прибыльные', '❌ Убыточные'], ['⚪ Безубыточные', '◻️ Без результата'], ['📊 Итоги', '🏠 Меню']])
        if cmd == '/pnl':
            return pnl_report(self.s.journal_path)
        if cmd == '/manual':
            return ('📘 Мануал TInvestBot\n\n'
                    '1. /status — состояние процесса; /balance — баланс; /portfolio — позиции.\n'
                    '2. /instruments — управление инструментами. Нажмите «Включить актив» и введите UID/FIGI, затем подтвердите.\n'
                    '3. /sync_instruments — каталог; /add UID — добавить выключенным. /enable UID и /disable UID — управление.\n'
                    '4. /pause и /resume — остановка/возобновление анализа. /mode observe|sandbox — режим.\n'
                    '5. /set interval N — период; /set max_order_lots N — размер заявки без фиксированного лимита; /settings — настройки.\n'
                    '6. /api — меню чтения методов T-Invest API. Методы с UID/текстом запрашивают параметр.\n'
                    '7. /learning — статистика решений. /tokens ТЕКСТ — оценка токенов Duck.\n'
                    '8. Все сделки остаются в Sandbox; локальная защита, проверка доступности и подтверждения не отключаются.\n\n'
                    'Для диагностики используйте /errors и /orders. Полный справочник API хранится в res/.')
        if cmd == '/tokens':
            if not args:
                return 'Формат: /tokens ТЕКСТ'
            result = await count_tokens(model=self.s.ai_model, system='Подсчёт токенов запроса.', messages=[{'role':'user','content':' '.join(args)}])
            return '🔢 Подсчёт токенов\n' + json.dumps(result, ensure_ascii=False, indent=2, default=str)[:4000]
        if cmd == '/learning':
            summary = LearningStore().summary()
            return ('🧠 Обучение на фактах\nНаблюдений: {observations}\nИнструментов: {instruments}\nДействия: {actions}\nРезультаты: {outcomes}\nПоследнее обновление: {last_update}\n\nКод и лимиты не изменяются автоматически; обучение только накапливает статистику.').format(**summary)
        if cmd == '/api':
            if not args:
                return Reply('🔌 API T-Invest\nВыберите раздел. Для методов с параметром бот попросит UID или текст.', [['👤 API: счета','ℹ️ API: профиль'],['💳 API: тариф','🛡 API: маржа'],['💸 API: вывод','🧾 API: операции'],['📉 API: рынок','🕯 API: свечи'],['📄 API: инструмент','🔍 API: поиск'],['💰 API: дивиденды','🧷 API: купоны'],['🗓 API: расписание','⭐ API: избранное'],['🌍 API: страны','⛔ API: стоп-заявки'],['🏠 Меню']])
            async with TBankClient(self.s) as b:
                api=BrokerFacade(b); kind=args[0].lower(); value=' '.join(args[1:])
                methods={'account':api.accounts,'info':api.user_info,'tariff':api.tariff,'margin':api.margin,'withdraw':api.withdraw_limits,'operations':api.operations,'market':lambda:api.market(value),'candles':lambda:api.candles(value),'instrument':lambda:api.instrument(value),'search':lambda:api.search(value),'dividends':lambda:api.dividends(value),'coupons':lambda:api.coupons(value),'schedule':lambda:api.schedules(value),'favorites':api.favorites,'countries':api.countries,'stoporders':api.stop_orders}
                if kind not in methods or (kind in {'market','candles','instrument','search','dividends','coupons'} and not value): return 'Формат: /api '+kind+' UID/текст'
                data=await methods[kind](); return '🔌 '+kind+'\n'+json.dumps(data,ensure_ascii=False,indent=2,default=str)[:12000]
        if cmd == '/balance' and not args:
            if not self.s.sandbox:
                return 'Production запрещён.'
            async with TBankClient(self.s) as b:
                portfolio = await b.portfolio()
                positions = await b.positions()
                return _balance_text(portfolio, positions)

        if cmd == '/id' and not args:
            return f'Ваш числовой Telegram ID: {user}'
        if cmd == '/vpn' and not args:
            return Reply('🌐 Прокси только для Telegram. T-Invest использует прямое соединение. После сохранения JSON требуется перезапуск Telegram.', [['📝 JSON прокси','🏠 Меню']])
        if cmd == '/pause' and not args:
            self.c.put('paused', True)
            return 'Пауза сохранена. Уже отправленные заявки НЕ отменены, позиции НЕ закрыты.'
        if cmd == '/resume' and not args:
            self.c.put('paused', False)
            return 'Анализ разрешён. Заявки — только в sandbox и при запуске daemon с --sandbox-trade.'
        if cmd == '/mode' and len(args) == 1 and args[0] in {'observe','sandbox'}:
            self.c.put('mode', args[0])
            return 'Режим: ' + ru(args[0])
        if cmd == '/notify' and len(args) == 1 and args[0] in {'all','important','off'}:
            self.c.put('notify', args[0])
            return 'Уведомления: ' + ru(args[0])
        if cmd == '/set' and len(args) == 2:
            self.c.set_limit(*args)
            return 'Настройка сохранена; применяется со следующего цикла. Текущие решения при изменении лимитов отклоняются.'
        if cmd == '/settings':
            lines = ['⚙️ Настройки', 'Пауза: '+('да' if self.c.get('paused',True) else 'нет'),
                     'Режим: '+ru(self.c.get('mode','observe')),
                     f"Цикл: {self.c.get('interval',60)} сек.",
                     f"Отчёты: {self.c.get('notify_interval',300)} сек.",
                     f"Лимит заявки: {self.c.get('max_order_lots',self.s.max_order_lots)} лотов",
                     f"Предел просадки: {self.c.get('max_drawdown_rub',self.s.max_drawdown_rub)} ₽",
                     f"Предел вложений: {self.c.get('max_exposure_rub',10000)} ₽",
                     'Уведомления: '+ru(self.c.get('notify','important'))]
            return Reply('\n'.join(lines), [['⏱ Цикл','⏲ Отчёты'], ['📦 Лимит лотов','🛡 Просадка','💼 Лимит вложений'], ['👁 Наблюдение','🧪 Песочница'], ['🔔 Все события','🔔 Важные','🔕 Без уведомлений'], ['👥 Друзья','🪪 Мой ID'], ['🌐 Прокси','🏠 Меню']])
        if cmd == '/status':
            p = Path('state/status.json')
            if not p.exists():
                return '📊 Статус\nТорговый процесс ещё не записал состояние. Запустите START_BOT.bat.'
            data = json.loads(p.read_text(encoding='utf-8'))
            return (f"📊 Последнее состояние\nОбновлено: {stamp(data.get('time'))}\n"
                    f"Этап: {ru(data.get('phase','неизвестен'))} · цикл {data.get('cycle','—')}\n"
                    f"Процесс: {data.get('pid','—')}\nПауза: {'да' if self.c.get('paused',True) else 'нет'}\n"
                    f"Режим: {ru(self.c.get('mode','observe'))}\n"
                    f"Без отправки заявок: {'да' if data.get('dry_run',True) else 'нет'}\n"
                    'Это сохранённый снимок, а не подтверждение работы процесса сейчас. Текущие лимиты — «Настройки».')
        if cmd == '/instruments':
            # Источник истины — Control.instruments(): его же использует daemon.
            # Просмотр меню не выполняет API-проверку и не меняет allowlist.
            catalog = self.c.get('instrument_catalog', {})
            current = self.c.instruments(self.s)
            lines, menu = [], {}
            for number, (uid, enabled) in enumerate(current.items(), 1):
                menu[str(number)] = uid
                item = catalog.get(uid, {})
                lines.append(f'{number}. {_instrument_line(item)} · '
                             + ('🟢 включён' if enabled else '⚪ выключен')
                             + f'\nUID/FIGI: {uid}')
            self.c.put('instrument_menu', menu)
            text = '🧰 Инструменты мониторинга (тот же список, что у daemon)\n' + ('\n\n'.join(lines) or 'Список пуст. Нажмите «Синхронизация».')
            text += '\n\nДля переключения: нажмите номер в чате или /instrument НОМЕР'
            return Reply(text, [['🔎 Поиск','➕ Добавить'], ['🟢 Включить актив','🔴 Выключить актив'], ['📚 Список из JSON','📥 Включить JSON'], ['🔄 Синхронизация','🏠 Меню']])
        if cmd == '/instrument' and len(args) == 1:
            menu = self.c.get('instrument_menu', {})
            uid = menu.get(args[0])
            if not uid:
                return 'Такого номера нет. Откройте /instruments и выберите номер из списка.'
            items = self.c.instruments(self.s)
            target = not bool(items.get(uid, False))
            action = '/enable' if target else '/disable'
            if target:
                self.pending[user] = (secrets.token_hex(4), time.monotonic()+60, '/enable', [uid])
                code = self.pending[user][0]
                return Reply(f'Включить инструмент №{args[0]} ({uid})?\n/confirm {code}', [['✅ Подтвердить','↩️ Отмена']])
            items[uid] = False
            self.c.put('instruments', items)
            return f'Инструмент №{args[0]} выключен. Новые заявки запрещены.'
        if cmd in {'/enable','/disable'} and len(args) == 1:
            items = self.c.instruments(self.s)
            if args[0] not in items:
                raise ValueError()
            if cmd == '/enable':
                async with TBankClient(self.s) as broker:
                    item = await broker.instrument(args[0])
                    if item.instrument_type != 'share' or item.currency.lower() != 'rub' or not item.buy_available_flag or not item.sell_available_flag:
                        return 'Нельзя включить: поддерживаются только доступные для торговли рублёвые акции.'
            items[args[0]] = cmd == '/enable'
            self.c.put('instruments', items)
            return 'Сохранено. Выключение запрещает новые заявки, но не отменяет существующие и не закрывает позиции.'
        if cmd == '/errors':
            j = Journal(self.s.journal_path)
            try:
                rows = j.db.execute("SELECT time,kind,payload FROM events WHERE kind IN ('ERROR','INSTRUMENT_ERROR','RISK_REJECTED') ORDER BY id DESC LIMIT 5").fetchall()
                return '⚠️ Последние ошибки\n'+('\n\n'.join(event_text(*row) for row in rows) if rows else 'Ошибок нет.')
            finally:
                j.close()
        if cmd in {'/portfolio','/orders','/search','/add','/cancel'}:
            if not self.s.sandbox:
                return 'Production запрещён.'
            async with TBankClient(self.s) as b:
                if cmd == '/portfolio':
                    return _portfolio_text(encode(await b.portfolio()))
                if cmd == '/orders':
                    orders = (await b.services.sandbox.get_sandbox_orders(account_id=self.s.account_id)).orders
                    lines = ['📋 Активные заявки · песочница']
                    for o in orders:
                        lines.append(f'ID: {o.order_id}\n{getattr(o, "figi", "—")} · {ru(getattr(o.direction, "name", o.direction))}\nЛоты: {o.lots_executed}/{o.lots_requested} исполнено\nСтатус: {ru(getattr(o.execution_report_status, "name", o.execution_report_status))}')
                    return Reply('\n\n'.join(lines) + ('\nАктивных заявок нет.' if not orders else ''), [['🛑 Отменить заявку','🏠 Меню']])
                if cmd == '/search' and args:
                    result = await b.services.instruments.find_instrument(query=' '.join(args))
                    return '\n'.join(f'{i.ticker} {i.class_code} | {i.name} | {i.uid}' for i in result.instruments[:20]) or 'Не найдено.'
                if cmd == '/add' and len(args) == 1:
                    item = await b.instrument(args[0])
                    if item.instrument_type != 'share' or item.currency.lower() != 'rub':
                        return 'Поддерживаются только рублёвые акции (не фьючерсы/облигации).'
                    if (not getattr(item, 'api_trade_available_flag', True)
                            or not getattr(item, 'buy_available_flag', True)
                            or not getattr(item, 'sell_available_flag', True)):
                        return 'Инструмент найден, но T-Bank не разрешает API-покупку и продажу.'
                    items = self.c.instruments(self.s)
                    items.setdefault(item.uid, False)
                    catalog = self.c.get('instrument_catalog', {})
                    catalog[item.uid] = {
                        'figi': getattr(item, 'figi', ''),
                        'ticker': getattr(item, 'ticker', ''),
                        'name': getattr(item, 'name', ''),
                        'currency': getattr(item, 'currency', 'rub'),
                    }
                    self.c.put('instrument_catalog', catalog)
                    self.c.put('instruments', items)
                    return f'{item.ticker}: {item.uid} добавлен выключенным. Откройте /instruments и нажмите номер или выполните /enable {item.uid}'
                if cmd == '/cancel' and len(args) == 1:
                    active = await b.services.sandbox.get_sandbox_orders(account_id=self.s.account_id)
                    if args[0] not in {o.order_id for o in active.orders}:
                        return 'Заявка не найдена среди активных; проверьте исполнение.'
                    await b.services.sandbox.cancel_sandbox_order(account_id=self.s.account_id, order_id=args[0])
                    return 'Запрос отмены принят. Проверьте /orders; частичное исполнение возможно.'
        return 'Неизвестная команда или параметры. /help'

class TelegramAPI:
    def __init__(self, token):
        self.base = 'https://api.telegram.org/bot' + token
        proxy = os.getenv('TELEGRAM_PROXY', '').strip() or None
        # A proxy is optional and is useful when api.telegram.org is blocked.
        self.client = httpx.AsyncClient(
            timeout=httpx.Timeout(40.0, connect=15.0),
            proxy=proxy,
            trust_env=True,
        )
        self.proxy_enabled = bool(proxy)

    async def call(self, method, **data):
        try:
            response = await self.client.post(self.base + '/' + method, json=data)
        except Exception as exc:
            logging.warning('TELEGRAM_HTTP_FAILED method=%s proxy=%s type=%s', method, self.proxy_enabled, type(exc).__name__)
            raise
        # Do not expose URLs containing bot token in exceptions/logging.
        if response.status_code != 200:
            raise RuntimeError(f'Telegram HTTP error {response.status_code}')
        result = response.json()
        if not result.get('ok'):
            description = str(result.get('description', 'unknown'))[:300]
            raise RuntimeError(f'Telegram API error: {description}')
        return result['result']

    async def send(self, chat, text, keyboard=False):
        # Bound command replies and event messages.
        markup = getattr(text, 'markup', None)
        for start in range(0, len(text), 3000):
            from html import escape
            chunk = str(text[start:start+3000])
            first, sep, rest = chunk.partition('\n')
            data = {'chat_id': chat, 'text': '<b>'+escape(first)+'</b>'+sep+escape(rest), 'parse_mode':'HTML'}
            if keyboard and start == 0:
                data['reply_markup'] = markup or KEYBOARD
            await self.call('sendMessage', **data)

async def notifications(s, c, api, users):
    while True:
        try:
            j = Journal(s.journal_path)
            try:
                j.db.execute('CREATE TABLE IF NOT EXISTS events (id INTEGER PRIMARY KEY, time TEXT, kind TEXT, payload TEXT)')
                for user in set(users) | set(c.get('telegram_friends', [])):
                    key = f'event_cursor_{user}'
                    cursor = c.get(key)
                    if cursor is None:
                        c.put(key, j.db.execute('SELECT COALESCE(MAX(id),0) FROM events').fetchone()[0])
                        continue
                    rows = j.db.execute('SELECT id,time,kind,payload FROM events WHERE id>? ORDER BY id LIMIT 10', (cursor,)).fetchall()
                    selected = []
                    mode = c.get('notify','important')
                    for row in rows:
                        if mode != 'off' and (row[2] in {'ORDER_SUBMITTED','ORDER_STATE','BROKER_OPERATION','ERROR','RISK_REJECTED','INSTRUMENT_ERROR','DAEMON_START','DAEMON_STOP'} or (mode == 'all' and row[2] == 'DECISION')):
                            selected.append(f'{row[1]} {row[2]}\n{row[3][:700]}')
                    if selected:
                        compact = [event_text(row[1],row[2],row[3]) for row in rows
                                   if mode != 'off' and (row[2] in {'ORDER_SUBMITTED','ORDER_STATE','BROKER_OPERATION','ERROR','RISK_REJECTED','INSTRUMENT_ERROR','DAEMON_START','DAEMON_STOP'} or (mode == 'all' and row[2] == 'DECISION'))]
                        await api.send(user, f'🔔 Последние события (интервал {c.get("notify_interval", 300)} сек):\n' + '\n\n'.join(compact))
                    if rows:
                        c.put(key, rows[-1][0])
            finally:
                j.close()
        except Exception as exc:
            logging.warning('TELEGRAM_NOTIFY_FAILED type=%s', type(exc).__name__)
        await asyncio.sleep(max(60, int(c.get('notify_interval', 300))))

async def run():
    token = os.getenv('TELEGRAM_BOT_TOKEN','').strip()
    users = {int(x.strip()) for x in os.getenv('TELEGRAM_ALLOWED_USER_IDS','').split(',') if x.strip()}
    if not token or not users or any(x <= 0 for x in users):
        raise SystemExit('Set TELEGRAM_BOT_TOKEN and positive TELEGRAM_ALLOWED_USER_IDS in .env')
    s = Settings()
    if not s.sandbox:
        raise SystemExit('Production is blocked')
    c = Control()
    commands = Commands(s,c,users)
    vpn_proc = None
    vpn_proxy = None
    try:
        vpn_proc, vpn_proxy = start_vpn()
    except Exception as exc:
        logging.warning('VPN_CONFIG_FAILED type=%s', type(exc).__name__)
    if vpn_proxy and not os.getenv('TELEGRAM_PROXY', '').strip():
        api = TelegramAPI(token)
        await api.client.aclose()
        os.environ['TELEGRAM_PROXY'] = vpn_proxy
    api = TelegramAPI(token)
    task = asyncio.create_task(notifications(s,c,api,users))
    try:
        me = await api.call('getMe')
        logging.info('TELEGRAM_CONNECTED username=%s proxy=%s', me.get('username'), api.proxy_enabled)
        await api.call('deleteWebhook', drop_pending_updates=False)
        while True:
            try:
                updates = await api.call('getUpdates', offset=c.get('telegram_offset',0), timeout=25, allowed_updates=['message'])
                for update in updates:
                    # At-most-once command execution: commit receipt before side effects.
                    c.put('telegram_offset', update['update_id']+1)
                    message = update.get('message',{})
                    reply = await commands.handle(message)
                    if reply:
                        await api.send(message['chat']['id'], reply, keyboard=message.get('from', {}).get('id') in commands.allowed_users())
            except Exception as exc:
                logging.warning('TELEGRAM_POLL_FAILED type=%s detail=%s', type(exc).__name__, str(exc)[:300])
                await asyncio.sleep(10)
    finally:
        task.cancel()
        await asyncio.gather(task, return_exceptions=True)
        await api.client.aclose()
        stop_vpn(vpn_proc)

def main():
    logging.basicConfig(level=logging.INFO)
    logging.getLogger('httpx').setLevel(logging.CRITICAL)
    logging.getLogger('httpcore').setLevel(logging.CRITICAL)
    with ProcessLock('state/telegram.lock'):
        try:
            asyncio.run(run())
        except KeyboardInterrupt:
            pass

if __name__ == '__main__':
    main()
