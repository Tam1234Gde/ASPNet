# Актуальный запуск

См. [RUNBOOK.md](RUNBOOK.md): t_tech.invest 1.51.0, read-only диагностика, безопасный --observe, логи и ограничения PnL. Старые инструкции ниже могут относиться к предыдущим версиям.

# TInvestBot — thin client T-Bank + API DUCK

Python 3.11+, asyncio. Локально нет моделей, обучения и торговой стратегии: приложение только собирает данные, передаёт snapshot внешней модели и выполняет проверенную директиву.

## Что реализовано

- Anthropic-compatible API DUCK (`https://apiduck.sytes.net`, `claude-sonnet-4-6`), JSON-парсинг и строгая валидация ответа.
- Snapshot: портфель, позиции, свечи, last prices, стакан и metadata инструмента.
- T-Bank SDK: Sandbox/Production-переключатель, Quotation/MoneyValue conversion, candles, order book, last prices, trading status, portfolio, заявки и order state.
- Allowlist UID, проверка торгового статуса, API/market/limit flags, lot ceiling, позиции и шага цены.
- SQLite-журнал конфигурации (заготовка для durable idempotency), запрет автоматического повторения заявки при неизвестном результате.
- Production намеренно заблокирован до завершения постоянного circuit breaker/flatten/reconciliation.
- По умолчанию `DRY_RUN=true`; тесты не создают заявки.

## Установка

```powershell
cd C:\Users\user\Desktop\Проекты\TInvestBot
.venv\Scripts\Activate.ps1
pip install -e ".[tinvest,test]"
Copy-Item .env.example .env
```

Заполните `.env`:

- `TINKOFF_INVEST_TOKEN` — токен T-Bank;
- `ACCOUNT_ID` — ID Sandbox-счёта;
- `AI_API_TOKEN` — ключ DUCK из интерфейса API HUB;
- `INSTRUMENT_UIDS` — UID инструмента через запятую, не FIGI.

Не публикуйте `.env`, ключ DUCK, T-Bank-токен и файлы из `res`, содержащие секреты. Если ключ уже был показан/скопирован, отзовите его и выпустите новый.

## Проверка AI без брокера

```powershell
.venv\Scripts\python.exe -m app.ai_check
```

Это вызывает DUCK, но не подключается к T-Bank и не создаёт заявку.

## Sandbox

1. Создайте Sandbox-счёт и получите `ACCOUNT_ID`.
2. Установите `IS_SANDBOX=true`, `DRY_RUN=false`.
3. Укажите UID инструмента.
4. Сначала пополните Sandbox и получите свечи:

```powershell
.venv\Scripts\python.exe smoke_test.py
```

5. Выполните один цикл:

```powershell
.venv\Scripts\python.exe -m app.main
```

Оставьте `ALLOW_MARKET_ORDERS=false` и `MAX_ORDER_LOTS=1`. Для сделки модель должна вернуть лимитную цену, кратную `min_price_increment`.

## Тесты и безопасная проверка

```powershell
.venv\Scripts\pytest.exe -q
.venv\Scripts\python.exe -m compileall -q app
.venv\Scripts\python.exe smoke_test.py
```

## Что ещё нельзя считать завершённым

Полноценный streaming `MarketDataStreamService`, запрос расписания `trading_schedules`, автоматическая отмена всех заявок и подтверждённый flatten при circuit breaker, постоянный portfolio reconciliation и расчёт realized PnL по операциям ещё требуют отдельной интеграции и тестирования. Поэтому Production-контур заблокирован. Feedback DUCK передаётся только если задан `AI_FEEDBACK_PATH`; документация `res` не описывает learning endpoint.

## Telegram и несколько инструментов

См. [подробную инструкцию](TELEGRAM_RUNBOOK.md). Новая версия по умолчанию на паузе в режиме observe. Запуск: START_TELEGRAM.bat + START_MANAGED_BOT.bat. Production заблокирован.
