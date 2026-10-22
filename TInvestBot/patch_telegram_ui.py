from pathlib import Path
p=Path('app/telegram.py'); s=p.read_text(encoding='utf-8')
a=s.index("        if cmd == '/transactions':"); b=s.index("        if cmd == '/balance'",a)
s=s[:a]+'''        if cmd == '/transactions':
            mode = args[0] if args and not args[0].isdigit() else 'all'
            page = int(args[-1]) if args and args[-1].isdigit() else 1
            text, page, pages = transaction_report(self.s.journal_path, mode, page)
            self.history[user] = (mode, page)
            return Reply(text, [['⬅️ Ранее', '➡️ Далее'], ['🧾 Все операции', '✅ Прибыльные', '❌ Убыточные'], ['⚪ Безубыточные', '◻️ Без результата'], ['📊 Итоги', '🏠 Меню']])
        if cmd == '/pnl':
            return pnl_report(self.s.journal_path)
        if cmd == '/learning':
            return LEARNING
'''+s[b:]
s=s.replace('from .telegram_views import report as transaction_report, pnl_report, LEARNING, event_text','from .telegram_views import report as transaction_report, pnl_report, LEARNING, event_text, Reply, ru, stamp')
s=s.replace('        self.pending = {}','        self.pending = {}\n        self.dialog = {}\n        self.history = {}')
a=s.index('        text = {',s.index('async def handle')); b=s.index("        if user in self.owners",a)
s=s[:a]+'''        aliases = {
            '📊 Статус':'/status', '💰 Баланс':'/balance', '📈 Портфель':'/portfolio',
            '🧾 Все операции':'/transactions', '🧾 Транзакции':'/transactions',
            '✅ Прибыльные':'/transactions profit', '❌ Убыточные':'/transactions loss',
            '⚪ Безубыточные':'/transactions zero', '◻️ Без результата':'/transactions unknown',
            '📊 Итоги':'/pnl', '🧰 Инструменты':'/instruments', '📚 Список из JSON':'/list_instruments',
            '🔄 Синхронизация':'/sync_instruments', '📋 Заявки':'/orders', '🧠 Обучение':'/learning',
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
            '/set max_order_lots': 'Введите лимит заявки: 1–10 лотов.',
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
'''+s[b:]
s=s.replace("return f'Подтвердите {cmd} {\" \".join(args)}: /confirm {code} (60 секунд)'", "return Reply(f'Подтвердите действие: {cmd} {\" \".join(args)}\\n/confirm {code} (60 секунд).\\nВключение актива и возобновление могут разрешить новые заявки в песочнице.', [['✅ Подтвердить', '↩️ Отмена']])")
s=s.replace("return 'Некорректные параметры. /help'", "return '⚠️ Некорректные параметры. Проверьте диапазон и формат в /help или повторно выберите кнопку. Изменения не подтверждены.'")
s=s.replace("return HELP\n        if cmd in", "return Reply(HELP, KEYBOARD['keyboard'])\n        if cmd in")
a=s.index("        if cmd == '/settings':"); b=s.index("        if cmd == '/instruments':",a)
s=s[:a]+'''        if cmd == '/settings':
            lines = ['⚙️ Настройки', 'Пауза: '+('да' if self.c.get('paused',True) else 'нет'),
                     'Режим: '+ru(self.c.get('mode','observe')),
                     f"Цикл: {self.c.get('interval',60)} сек.",
                     f"Отчёты: {self.c.get('notify_interval',300)} сек.",
                     f"Лимит заявки: {self.c.get('max_order_lots',self.s.max_order_lots)} лотов",
                     f"Предел просадки: {self.c.get('max_drawdown_rub',self.s.max_drawdown_rub)} ₽",
                     f"Предел вложений: {self.c.get('max_exposure_rub',10000)} ₽",
                     'Уведомления: '+ru(self.c.get('notify','important'))]
            return Reply('\\n'.join(lines), [['⏱ Цикл','⏲ Отчёты'], ['📦 Лимит лотов','🛡 Просадка','💼 Лимит вложений'], ['👁 Наблюдение','🧪 Песочница'], ['🔔 Все события','🔔 Важные','🔕 Без уведомлений'], ['👥 Друзья','🪪 Мой ID'], ['🌐 Прокси','🏠 Меню']])
        if cmd == '/status':
            p = Path('state/status.json')
            if not p.exists():
                return '📊 Статус\\nТорговый процесс ещё не записал состояние. Запустите START_BOT.bat.'
            data = json.loads(p.read_text(encoding='utf-8'))
            return (f"📊 Последнее состояние\\nОбновлено: {stamp(data.get('time'))}\\n"
                    f"Этап: {ru(data.get('phase','неизвестен'))} · цикл {data.get('cycle','—')}\\n"
                    f"Процесс: {data.get('pid','—')}\\nПауза: {'да' if self.c.get('paused',True) else 'нет'}\\n"
                    f"Режим: {ru(self.c.get('mode','observe'))}\\n"
                    f"Без отправки заявок: {'да' if data.get('dry_run',True) else 'нет'}\\n"
                    'Это сохранённый снимок, а не подтверждение работы процесса сейчас. Текущие лимиты — «Настройки».')
'''+s[b:]
s=s.replace("return 'Режим: ' + args[0]", "return 'Режим: ' + ru(args[0])")
s=s.replace("return 'Уведомления: ' + args[0]", "return 'Уведомления: ' + ru(args[0])")
s=s.replace("lines.append(_instrument_line(item))", "lines.append(_instrument_line(item) + (' · 🟢 включён' if self.c.instruments(self.s)[uid] else ' · ⚪ выключен') + '\\nUID/FIGI: ' + uid)")
s=s.replace("return '\\n'.join(lines) or 'Список пуст. /enable_list — импортировать файл.'", "return Reply('🧰 Инструменты\\n'+('\\n'.join(lines) or 'Список пуст.'), [['🔎 Поиск','➕ Добавить'], ['🟢 Включить актив','🔴 Выключить актив'], ['📚 Список из JSON','📥 Включить JSON'], ['🔄 Синхронизация','🏠 Меню']])")
s=s.replace("return json.dumps(rows, ensure_ascii=False) if rows else 'Ошибок нет.'", "return '⚠️ Последние ошибки\\n'+('\\n\\n'.join(event_text(*row) for row in rows) if rows else 'Ошибок нет.')")
s=s.replace("return 'Друзья: ' + (', '.join(map(str, self.c.get('telegram_friends', []))) or 'нет')", "return Reply('👥 Друзья: ' + (', '.join(map(str, self.c.get('telegram_friends', []))) or 'нет'), [['👤 Дать доступ','🚫 Отозвать доступ'], ['🏠 Меню']])")
s=s.replace("return 'Прокси задаётся только для Telegram. T-Invest API использует прямое соединение. Для настройки: /vpn_json + JSON, затем перезапустите Telegram.'", "return Reply('🌐 Прокси только для Telegram. T-Invest использует прямое соединение. После сохранения JSON требуется перезапуск Telegram.', [['📝 JSON прокси','🏠 Меню']])")
s=s.replace("return json.dumps(encode(await b.services.sandbox.get_sandbox_orders(account_id=self.s.account_id)), ensure_ascii=False)", "orders = (await b.services.sandbox.get_sandbox_orders(account_id=self.s.account_id)).orders\n                    lines = ['📋 Активные заявки · песочница']\n                    for o in orders:\n                        lines.append(f'ID: {o.order_id}\\n{getattr(o, \"figi\", \"—\")} · {ru(getattr(o.direction, \"name\", o.direction))}\\nЛоты: {o.lots_executed}/{o.lots_requested} исполнено\\nСтатус: {ru(getattr(o.execution_report_status, \"name\", o.execution_report_status))}')\n                    return Reply('\\n\\n'.join(lines) + ('\\nАктивных заявок нет.' if not orders else ''), [['🛑 Отменить заявку','🏠 Меню']])")
s=s.replace("        text = text[:12000]\n", "        markup = getattr(text, 'markup', None)\n")
s=s.replace("data = {'chat_id': chat, 'text': text[start:start+3000]}", "from html import escape\n            chunk = str(text[start:start+3000])\n            first, sep, rest = chunk.partition('\\n')\n            data = {'chat_id': chat, 'text': '<b>'+escape(first)+'</b>'+sep+escape(rest), 'parse_mode':'HTML'}")
s=s.replace("data['reply_markup'] = KEYBOARD", "data['reply_markup'] = markup or KEYBOARD")
a=s.index('                        compact = []'); b=s.index('                    if rows:',a)
s=s[:a]+'''                        compact = [event_text(row[1],row[2],row[3]) for row in rows
                                   if mode != 'off' and (row[2] in {'ORDER_SUBMITTED','ORDER_STATE','BROKER_OPERATION','ERROR','RISK_REJECTED','INSTRUMENT_ERROR','DAEMON_START','DAEMON_STOP'} or (mode == 'all' and row[2] == 'DECISION'))]
                        await api.send(user, f'🔔 Последние события (интервал {c.get("notify_interval", 300)} сек):\\n' + '\\n\\n'.join(compact))
'''+s[b:]
s=s.replace("/transactions [profit|loss] — история операций и результат", "/transactions [all|profit|loss|zero|unknown] [страница] — вся сохранённая история\n/pnl — общие итоги и результат по активам\n/abort — отменить ввод или подтверждение\n/errors — последние ошибки\n/set max_drawdown_rub ЧИСЛО — предел просадки, 1–1000000 ₽\n/set max_exposure_rub ЧИСЛО — предел вложений, 1–10000000 ₽")
p.write_text(s,encoding='utf-8')
