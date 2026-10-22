from pathlib import Path
p = Path('app/main.py')
s = p.read_text(encoding='utf-8')
s = s.replace('    item = metadata[d.instrument_id]', '''    from pathlib import Path
    stop_path = Path('state/stop.request')
    if stop_path.exists() and stop_path.read_text(encoding='utf-8-sig').strip() == 'stop':
        logging.info('STOP_REQUESTED: no new order')
        return
    book = books[d.instrument_id]
    book_time = book.get('orderbook_ts') or book.get('time')
    if not book_time or (datetime.now(timezone.utc) - datetime.fromisoformat(book_time)).total_seconds() > s.max_snapshot_age_seconds:
        raise ValueError('Orderbook missing timestamp or stale')
    item = metadata[d.instrument_id]''')
s = s.replace('    CircuitBreaker(Decimal', '''    if d.action == Action.BUY and lots + d.lots > s.max_order_lots:
        raise ValueError('Total position ceiling exceeded')
    CircuitBreaker(Decimal''')
p.write_text(s, encoding='utf-8')
