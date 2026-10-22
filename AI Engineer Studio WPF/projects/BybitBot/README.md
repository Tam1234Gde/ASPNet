# Bybit ML Bot (Россия)

> ⚠️ Торговля фьючерсами несёт риск полной потери средств. Начинайте только с **Testnet**. Этот проект — технический шаблон, не финансовая рекомендация.

## Возможности

.NET 8 Worker для USDT-линейных perpetual-контрактов Bybit V5: Testnet/Mainnet через конфигурацию, HMAC-подпись, повтор запросов при 429/5xx, WebSocket закрытых минутных свечей, ONNX inference, SL/TP для каждого ордера, sizing по риску, один активный position, дневной лимит убытка и cooldown.

## Требования

* .NET SDK 8+;
* Python 3.10+ (для демонстрационной модели);
* ключи Bybit V5. Testnet: https://testnet.bybit.com → Account → API. Live: https://www.bybit.com → API. **Demo Trading и Testnet — разные среды и ключи**.

## Быстрый запуск

```powershell
python -m venv .venv
.\.venv\Scripts\activate
pip install numpy scikit-learn skl2onnx onnx
python .\train_model.py
dotnet restore
dotnet test .\tests\BybitBot.Tests\BybitBot.Tests.csproj
dotnet run
```

В `appsettings.json` заполните `Bot:Bybit:ApiKey` и `ApiSecret`, не коммитьте их. Безопаснее задавать переменными окружения PowerShell:

```powershell
$env:Bot__Bybit__ApiKey='ключ'
$env:Bot__Bybit__ApiSecret='секрет'
$env:Bot__Bybit__Environment='Testnet'
dotnet run
```

`Environment=Testnet` использует `api-testnet.bybit.com` и `stream-testnet.bybit.com`; `Mainnet` — live endpoints. Модель принимает `[1,5]`: log-return, RSI, ATR/price, normalized spread, изменение close. Выход — `probabilities [Flat, Long, Short]`.

## Настройка риска

В `Bot:Risk`: `MaxRiskPercent` (0.01 = 1% баланса на SL), `DailyMaxLossPercent`, `MaxPositionCount`, ATR-множители SL/TP. В `Trading`: пара, confidence и cooldown. Перед торговлей добавьте проверку тик-шага/минимального количества через `/v5/market/instruments-info` для выбранного инструмента: значения различаются по символам.

## Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet","BybitBot.dll"]
```

Сохраните как `Dockerfile`, затем: `docker build -t bybitbot .` и `docker run --rm -e Bot__Bybit__ApiKey=... -e Bot__Bybit__ApiSecret=... bybitbot`.

## Чек-лист до Mainnet

1. Несколько дней проверьте Testnet и журнал ордеров.
2. Используйте отдельный ключ без прав вывода, IP whitelist.
3. Проверьте symbol, leverage, tick/lot size и наличие средств.
4. Убедитесь, что `Environment=Mainnet`: программа печатает красный баннер.
5. Начните с минимального размера и ограничений риска ниже 1%.
6. Не считайте dummy-модель прибыльной: обучите/валидируйте модель на исторических данных без lookahead bias.
