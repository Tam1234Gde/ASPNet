from pathlib import Path
p=Path('app/telegram.py')
s=p.read_text(encoding='utf-8')
s=s.replace("encoding='utf-8'))", "encoding='utf-8-sig'))",1)
a=s.index('def _money('); b=s.index('\nclass Commands:',a)
s=s[:a]+'''def _field(obj, *names, default=None):
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


def _balance_text(portfolio, positions=None):
    lines = ['💰 Счёт Sandbox', 'Стоимость портфеля: ' + _money(_field(portfolio, 'total_amount_portfolio')),
             'Валютная часть: ' + _money(_field(portfolio, 'total_amount_currencies'))]
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
    return '\\n'.join(lines)


def _portfolio_text(portfolio):
    lines = [_balance_text(portfolio)]
    positions = _field(portfolio, 'positions', default=[]) or []
    lines.append(f'\\n📈 Позиции: {len(positions)}')
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
    return '\\n'.join(lines)


def _instrument_line(item):
    currency = str(item.get('currency', '')).lower()
    label = 'рублёвый' if currency == 'rub' else 'не рублёвый' if currency else 'валюта не указана'
    return f"{item.get('figi') or 'FIGI не указан'} — {label}"

''' + s[b:]
s=s.replace('self.pending = {}', '''self.pending = {}
        self.owners = set(users)

    def allowed_users(self):
        return self.owners | set(self.c.get('telegram_friends', []))''',1)
s=s.replace("if user not in self.users or chat.get('type') != 'private' or chat.get('id') != user:", "if chat.get('type') != 'private' or chat.get('id') != user:")
s=s.replace("        # Never execute", "        if user not in self.allowed_users():\n            return f'Ваш Telegram ID: {user}. Попросите владельца выполнить /friend_add {user}.' if message.get('text', '').split('@')[0] in {'/id', '/start'} else None\n        # Never execute",1)
s=s.replace("if user in getattr(self, 'vpn_pending', set())", "if user in self.owners and user in getattr(self, 'vpn_pending', set())")
s=s.replace("        try:\n            if cmd == '/vpn_json':", "        try:\n            if cmd in {'/vpn_json', '/vpn', '/friend_add', '/friend_remove', '/friends', '/set', '/mode', '/notify'} and user not in self.owners:\n                return 'Эта команда доступна только владельцу.'\n            if cmd == '/vpn_json':",1)
s=s.replace("{'/resume','/enable','/set','/cancel'}", "{'/resume','/enable','/enable_list','/set','/cancel','/friend_add'}")
s=s.replace("        if cmd in {'/help', '/start'}:", """        if user is not None and user not in self.allowed_users():
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
            return 'Друзья: ' + (', '.join(map(str, self.c.get('telegram_friends', []))) or 'нет')
        if cmd in {'/help', '/start'}:""",1)
a=s.index("            rows = [x for x in source");b=s.index("        if cmd == '/sync_instruments'",a)
s=s[:a]+'''            rows = [x for x in source if isinstance(x, dict) and (x.get('uid') or x.get('figi'))]
            if cmd == '/list_instruments':
                return '\\n'.join(_instrument_line(x) for x in rows) or 'Список пуст.'
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
                    if not current[key] and sum(bool(v) for v in current.values()) >= 30:
                        limited += 1
                        continue
                    current[key] = True
                    enabled += 1
            self.c.put('instrument_catalog', catalog)
            self.c.put('instruments', current)
            return (f'В каталоге из файла: {len(rows)}. Включено для торговли: {enabled}. '
                    f'Не поддерживаются стратегией/неторгуемые: {unavailable}. Ошибки API: {failed}. Лимит: {limited}. '
                    'Торговля поддерживает только рублёвые акции; индексы и сырьевые индикаторы не включаются. /instruments')
''' + s[b:]
s=s.replace("return _portfolio_text(portfolio).split('\\n📈 Позиции:', 1)[0]", "positions = await b.positions()\n                return _balance_text(portfolio, positions)")
a=s.index("            return 'Лимит: максимум 10 лотов");b=s.index("        if cmd in {'/enable','/disable'}",a)
s=s[:a]+'''            catalog = self.c.get('instrument_catalog', {})
            source = {x.get('uid') or x.get('figi'): x for x in _load_instrument_list() if isinstance(x, dict)}
            lines = []
            async with TBankClient(self.s) as broker:
                for uid in self.c.instruments(self.s):
                    item = catalog.get(uid) or source.get(uid)
                    if not item:
                        try:
                            resolved = await broker.instrument(uid)
                            item = {'figi': resolved.figi, 'currency': resolved.currency}
                            catalog[uid] = item
                        except Exception:
                            item = {'figi': uid if len(uid) == 12 else None}
                    lines.append(_instrument_line(item))
            self.c.put('instrument_catalog', catalog)
            return '\\n'.join(lines) or 'Список пуст. /enable_list — импортировать файл.'
''' + s[b:]
s=s.replace("            items[args[0]] = cmd == '/enable'", """            if cmd == '/enable':
                async with TBankClient(self.s) as broker:
                    item = await broker.instrument(args[0])
                    if item.instrument_type != 'share' or item.currency.lower() != 'rub' or not item.buy_available_flag or not item.sell_available_flag:
                        return 'Нельзя включить: поддерживаются только доступные для торговли рублёвые акции.'
                if not items[args[0]] and sum(bool(v) for v in items.values()) >= 30:
                    return 'Максимум 30 включённых инструментов.'
            items[args[0]] = cmd == '/enable'""")
s=s.replace("for user in users:", "for user in set(users) | set(c.get('telegram_friends', [])):")
s=s.replace("'Транзакции за последние 5 минут:\\n'", "f'Последние события (интервал {c.get(\"notify_interval\", 300)} сек):\\n'")
s=s.replace("keyboard=message.get('text','').strip() in {'/start','/help'}", "keyboard=message.get('from', {}).get('id') in commands.allowed_users()")
s=s.replace('/instruments — мои инструменты: ON/OFF и лимит лотов', '/instruments — каталог: FIGI и рублёвый или нет')
s=s.replace('/enable_list — добавить и включить доступные инструменты из instrumentlist.json', '/enable_list — импортировать файл и включить доступные рублёвые акции (подтверждение)')
s=s.replace('/id — показать Telegram ID', '/id — показать Telegram ID\n/friend_add ID — дать другу управление (владелец, подтверждение)\n/friend_remove ID — отозвать доступ (владелец)\n/friends — список друзей (владелец)\nДруг: просмотр счёта, инструменты, пауза/запуск, отмена заявок.\nНастройки, режим, прокси и выдача доступа — только владельцу.')
p.write_text(s,encoding='utf-8')
p=Path('app/tbank.py');s=p.read_text(encoding='utf-8');s=s.replace('    async def instrument(self, uid):', '''    async def positions(self):
        if self.s.sandbox:
            return await self.services.sandbox.get_sandbox_positions(account_id=self.s.account_id)
        return await self.services.operations.get_positions(account_id=self.s.account_id)

    async def instrument(self, uid):''');s=s.replace('uid.startswith("BBG")', 'len(uid) == 12');p.write_text(s,encoding='utf-8')
