"""Получение и фильтрация доступных торговых инструментов."""
from .tbank import TBankClient


def _tradeable(item):
    """Критерий фактической поддержки торговым контуром бота.

    Стратегия работает только с рублёвыми акциями, по которым T-Bank
    разрешает API-покупку и API-продажу. ETF/облигации/информационные записи
    не попадают в список выбора и не могут случайно включиться.
    """
    return (
        getattr(item, 'instrument_type', None) == 'share'
        and str(getattr(item, 'currency', '')).lower() == 'rub'
        and bool(getattr(item, 'api_trade_available_flag', True))
        and bool(getattr(item, 'buy_available_flag', True))
        and bool(getattr(item, 'sell_available_flag', True))
    )


async def fetch_rub_instruments(settings):
    """Возвращает рублёвые акции/ETF/облигации, доступные для API-торговли."""
    async with TBankClient(settings) as bank:
        svc = bank.services.instruments
        result = []
        for getter in (svc.shares, svc.etfs, svc.bonds):
            response = await getter()
            for item in getattr(response, 'instruments', []):
                if _tradeable(item):
                    result.append(item)
        unique = {item.uid: item for item in result if getattr(item, 'uid', None)}
        return sorted(unique.values(), key=lambda x: (getattr(x, 'ticker', ''), getattr(x, 'name', '')))
