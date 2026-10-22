"""Russian presentation and read-only, paginated operation reports."""
import json
import sqlite3
from pathlib import Path
from decimal import Decimal
from datetime import datetime, timezone
from collections import defaultdict
from .statistics import calculate

class Reply(str):
    def __new__(cls, text, rows=None):
        obj = super().__new__(cls, text)
        obj.markup = ({'keyboard': rows, 'resize_keyboard': True, 'is_persistent': True}
                      if rows is not None else None)
        return obj

LEARNING = ('🧠 Как работает AI\n\nСамообучения и изменения весов модели в проекте нет. '
            'Внешняя модель последовательно анализирует каждый включённый актив. '
            'Один AI-клиент получает общий контекст последних 20 feedback-событий разных активов. '
            'Контекст хранится в памяти и теряется при перезапуске; это не дообучение. '
            'Отдельный запрос feedback, если настроен, тоже не доказывает обучение на стороне провайдера. '
            'Журнал сделок и FIFO нужны для отчётности. Поддерживаются рублёвые акции; '
            'индексы и сырьевые индикаторы из JSON не становятся торгуемыми активами.')

KINDS = {'BUY':'Покупка', 'SELL':'Продажа', 'INPUT':'Пополнение', 'OUTPUT':'Вывод',
         'BROKER_FEE':'Комиссия брокера', 'SERVICE_FEE':'Плата за обслуживание',
         'DIVIDEND':'Дивиденды', 'COUPON':'Купон', 'TAX':'Налог'}
EVENTS = {'ERROR':'Ошибка цикла', 'INSTRUMENT_ERROR':'Ошибка анализа актива',
          'RISK_REJECTED':'Заявка отклонена защитой', 'DAEMON_START':'Торговый процесс запущен',
          'DAEMON_STOP':'Торговый процесс остановлен', 'ORDER_SUBMITTED':'Заявка отправлена',
          'ORDER_STATE':'Обновление заявки', 'BROKER_OPERATION':'Операция брокера',
          'DECISION':'Решение AI'}
VALUES = {'BUY':'покупка', 'SELL':'продажа', 'HOLD':'ожидание', 'CLOSE':'закрытие',
          'observe':'наблюдение без заявок', 'sandbox':'песочница', 'all':'все события',
          'important':'важные события', 'off':'выключены', 'waiting':'ожидание цикла',
          'running':'работает', 'starting':'запуск', 'stopped':'остановлен',
          'ConnectError':'ошибка подключения', 'ReadTimeout':'тайм-аут ответа',
          'TimeoutError':'истекло время ожидания', 'ValueError':'ошибка проверки данных'}

def ru(value):
    text = str(value)
    for prefix in ('EXECUTION_REPORT_STATUS_', 'ORDER_DIRECTION_'):
        if text.startswith(prefix):
            text = text[len(prefix):]
    return {**VALUES, 'FILL':'исполнена', 'NEW':'активна', 'PARTIALLYFILL':'частично исполнена',
            'CANCELLED':'отменена', 'REJECTED':'отклонена'}.get(text, text)

def stamp(value):
    try:
        return datetime.fromisoformat(str(value).replace('Z','+00:00')).astimezone(timezone.utc).strftime('%d.%m.%Y %H:%M UTC')
    except (ValueError, TypeError):
        return str(value or 'время неизвестно')

def amount(value, currency='rub'):
    if value is None or value == '':
        return 'не рассчитан'
    return f'{Decimal(str(value)):+,.2f}'.replace(',', ' ') + (' ₽' if currency.lower() == 'rub' else ' ' + currency.upper())

def load_operations(path):
    if not Path(path).exists():
        return []
    with sqlite3.connect(Path(path).resolve().as_uri() + '?mode=ro', uri=True) as db:
        if not db.execute("SELECT 1 FROM sqlite_master WHERE type='table' AND name='operations'").fetchone():
            return []
        return [json.loads(row[0]) for row in db.execute('SELECT payload FROM operations')]

def report(path, mode='all', page=1):
    if mode not in {'all','profit','loss','zero','unknown'} or page < 1:
        raise ValueError('Неверный фильтр или страница')
    operations = load_operations(path)
    summary, rows = calculate(operations)
    by_id = {x['id']: x for x in operations}
    # calculate includes executed operations only; cancelled/pending must remain visible.
    present = {x['id'] for x in rows}
    for x in operations:
        if x['id'] not in present:
            from .statistics import money
            rows.append({'id':x['id'], 'date':x['date'], 'type':x.get('operation_type',''),
                         'instrument':x.get('instrument_uid') or x.get('figi',''),
                         'currency':x.get('currency',''), 'quantity':x.get('quantity',0),
                         'payment':str(money(x.get('payment',{}))), 'fifo_gross_pnl_rub':''})
    def category(r):
        p = r.get('fifo_gross_pnl_rub','')
        return 'unknown' if p == '' else 'profit' if Decimal(p)>0 else 'loss' if Decimal(p)<0 else 'zero'
    selected = sorted([r for r in rows if mode == 'all' or category(r)==mode], key=lambda r:(r['date'],r['id']), reverse=True)
    pages = max(1, (len(selected)+5)//6)
    page = min(page, pages)
    lines = [f'🧾 История операций · {page}/{pages}',
             'Источник: сохранённые операции брокера, не журнал сигналов AI.',
             f'Всего: {len(rows)} · по фильтру: {len(selected)}',
             'Метки прибыли/убытка — FIFO ДО комиссий и налогов.']
    for r in selected[(page-1)*6:page*6]:
        x = by_id[r['id']]
        kind = KINDS.get(r['type'].replace('OPERATION_TYPE_',''), x.get('type') or 'Прочая операция')
        p = r.get('fifo_gross_pnl_rub','')
        result = {'profit':'🟢 Прибыль', 'loss':'🔴 Убыток', 'zero':'⚪ Безубыточно', 'unknown':'◻️ Результат не определён'}[category(r)]
        if p != '':
            result += ': '+amount(p)
        elif kind == 'Покупка':
            result = '◻️ Покупка: результат фиксируется при продаже'
        elif kind != 'Продажа':
            result = '◻️ Не закрытая продажа: метка прибыли не применяется'
        state = {'OPERATION_STATE_EXECUTED':'исполнено','OPERATION_STATE_CANCELED':'отменено','OPERATION_STATE_PROGRESS':'в процессе'}.get(x.get('state'), 'статус неизвестен')
        lines += ['', f'{stamp(r["date"])} · {kind} · {state}',
                  f'{x.get("ticker") or x.get("figi") or r["instrument"] or "Счёт"} · {r["quantity"]} шт.',
                  'Денежный поток: '+amount(r['payment'], r['currency']), result, 'ID: '+r['id']]
    if not selected:
        lines.append('\nПодходящих операций пока нет.')
    lines.append('\nПолнота ограничена загруженной историей; обновление — торговым процессом. Пополнение не является прибылью.')
    return '\n'.join(lines), page, pages

def pnl_report(path):
    summary, rows = calculate(load_operations(path))
    totals = defaultdict(lambda: [Decimal(0),0,0,0,0])
    for r in rows:
        if r['type'] != 'OPERATION_TYPE_SELL':
            continue
        t = totals[r['instrument']]
        p = r.get('fifo_gross_pnl_rub','')
        if p == '':
            t[4] += 1
        else:
            p = Decimal(p)
            t[0] += p
            t[1 if p>0 else 2 if p<0 else 3] += 1
    lines = ['📊 Итоги сохранённой истории', 'FIFO до комиссий: '+amount(summary.get('fifo_gross_pnl_rub')),
             'FIFO с привязанными комиссиями: '+amount(summary.get('realized_pnl_rub')),
             'Все комиссии брокера: '+amount(summary.get('broker_fees_rub')),
             'Пополнения минус выводы (не прибыль): '+amount(summary.get('net_cashflows_rub')),
             '\nПо активам · до комиссий:']
    for key, t in sorted(totals.items()):
        lines.append(f'{key}: {amount(t[0])} по рассчитанным продажам; 🟢 {t[1]} / 🔴 {t[2]} / ⚪ {t[3]} / неизвестно {t[4]}')
    if not totals:
        lines.append('Закрытых продаж нет.')
    lines.append('\nПредварительный FIFO, не налоговый отчёт. Неполная себестоимость, непривязанные комиссии и неподдержанные операции делают общий результат недоступным. Поздние комиссии меняют расчёт. Открытые позиции — в «Портфель».')
    return '\n'.join(lines)

def event_text(when, kind, payload):
    try:
        data = json.loads(payload) if isinstance(payload,str) else payload
    except ValueError:
        data = {}
    details = []
    for key,label in [('instrument','Актив'),('instrument_id','Актив'),('action','Действие'),('status','Статус'),('type','Тип ошибки'),('reason','Причина'),('lots','Лоты'),('cycle','Цикл')]:
        if key in data:
            details.append(label+': '+ru(data[key]))
    return stamp(when)+' · '+EVENTS.get(kind,'Событие')+'\n'+('\n'.join(details) or 'Подробности доступны в локальном журнале.')
