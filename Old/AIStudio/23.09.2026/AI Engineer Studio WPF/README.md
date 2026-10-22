# AI Engineer Studio WPF

WPF IDE для общения с OpenAI-compatible нейросетью и передачи контекста файлов проекта.

## Запуск

Из каталога проекта:

```powershell
cd "AI Engineer Studio WPF"
python -m pip install openai
dotnet run --project AIEngineerStudio.Wpf.csproj
```

Если `python` не находится, путь можно добавить в настройку Python после реализации соответствующего поля UI или указать Python в PATH.

## API и настройки

Откройте `⚙` в Activity Bar. Доступны:

- API-токен;
- Base URL;
- модель;
- тема;
- temperature;
- max output tokens;
- системный промпт.

Настройки сохраняются в `%AppData%\AIEngineerStudio\settings.json`. API-токен защищается Windows DPAPI для текущего пользователя.

Кнопка `☀` быстро переключает Graphite и светлую тему.

## Работа

1. Выберите папку проекта через `+ Добавить папку`.
2. Дважды щёлкните файлы в Solution Explorer для включения в контекст.
3. Выберите модель и промпт.
4. Введите многострочный запрос.
5. Нажмите «Отправить» или `Ctrl+Enter`.
6. Ответ отображается с Markdown-заголовками, bold, списками и fenced code blocks.

## Проверки

```powershell
cd "AI Engineer Studio WPF"
python -m py_compile tools/neural_bridge.py
dotnet build AIEngineerStudio.Wpf.csproj
```

Подробный контекст: `PROJECT_CONTEXT.md`. Незакрытые задачи: `KNOWN_ISSUES.md`.
