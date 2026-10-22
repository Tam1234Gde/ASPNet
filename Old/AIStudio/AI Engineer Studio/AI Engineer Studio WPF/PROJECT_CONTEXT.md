# AI Engineer Studio WPF — актуальный контекст проекта

Дата обновления: 2026-09-22.

## Назначение
Desktop IDE для работы с OpenAI-compatible нейросетью в стиле Visual Studio / VS Code. Приложение работает с контекстом файлов, историей сообщений, Markdown-ответами, Diff/Preview и настройками темы/API.

## Стек
- .NET 10 / WPF / C#.
- Borderless WPF Window с собственной titlebar.
- `neural_bridge.py` + Python 3 + пакет `openai`.
- DPAPI (`ProtectedData`, CurrentUser) для сохранения API-токена.
- Локальный Markdown renderer без внешней Markdown-библиотеки.

## Файлы
- `AIEngineerStudio.Wpf.csproj` — WPF-проект; копирует bridge в output.
- `App.xaml` — DynamicResource-палитра, кастомные Button, ComboBox, ScrollBar и TreeViewItem.
- `App.xaml.cs` — точка входа.
- `MainWindow.xaml` — titlebar, activity bar, Solution Explorer, чат, inspector, status bar.
- `MainWindow.xaml.cs` — состояние UI, темы, Solution Explorer, история, вызов bridge, Markdown, Diff и Preview.
- `Views/Dialogs.cs` — настройки API/модели/параметров/темы/промпта и библиотека промптов.
- `Models/ChatModels.cs` — DTO сообщений и запросов.
- `Services/ProjectFileService.cs` — перечисление файлов и сбор контекста.
- `Services/NeuralBridgeService.cs` — запуск bridge и обработка ответа API.
- `UI/ThemeManager.cs`, `UI/MarkdownRenderer.cs` — темы и Markdown-отображение.
- `Services/SettingsStore.cs` — загрузка/сохранение настроек в `%AppData%\AIEngineerStudio\settings.json`; API-токен шифруется DPAPI.
- `neural_bridge.py` — JSON stdin/stdout bridge для OpenAI-compatible API.
- `README.md` — запуск и пользовательская инструкция.
- `DEVELOPMENT.md` — правила разработки.
- `KNOWN_ISSUES.md` — актуальный список незакрытых проблем.

## Запрос к нейросети
1. Пользователь дважды щёлкает файл в Solution Explorer.
2. `BuildContext()` собирает до 15 выбранных файлов, каждый ограничен примерно 12000 символами.
3. История сообщений передаётся в bridge.
4. C# запускает Python bridge, отправляет JSON через stdin.
5. Bridge вызывает `OpenAI(api_key, base_url).chat.completions.create(...)`.
6. Ответ JSON отображается локальным Markdown renderer.

## UI-функции
- Solution Explorer: корень проекта, вложенные папки, иконки по расширениям, отметки контекстных файлов.
- Custom titlebar: drag, minimize, maximize, close.
- Настройки через `⚙`: API-токен, Base URL, модель, тема, температура, max tokens, системный промпт.
- Быстрая тема через `☀`: переключение Graphite ↔ Светлая.
- Темы: Graphite, Светлая, Midnight Blue, Emerald Night.
- Кнопки используют кастомный шаблон без стандартной синей WPF focus/hover-подсветки; остаётся только press-анимация.
- ComboBox и ScrollBar стилизованы через глобальные шаблоны.
- Inspector: Agent Actions, Diff Viewer, Live Preview.

## Правила дальнейшей работы
- Перед изменением читать этот файл и `KNOWN_ISSUES.md`.
- Не редактировать `bin`, `obj`, `.vs`, `__pycache__`.
- Использовать `DynamicResource`, не добавлять жёсткие цвета в новые контролы.
- Не возвращать системные WPF-шаблоны для Button/ComboBox/ScrollBar.
- Не хранить API-токен в исходниках.
- Реальные операции write/execute агента должны проходить approval flow.
- После изменений запускать `dotnet build` из каталога WPF и `python -m py_compile neural_bridge.py`.

## Проектный контекст v2
- Проекты хранятся в `Projects/` рядом с опубликованным приложением.
- Каждый проект — JSON-документ с именем, рабочей папкой, описанием, временем обновления и историей чата.
- При запуске открывается диалог выбора существующего или создания нового проекта.
- Контекстная панель скрыта до выбора проекта.
- После ответа ассистента история сохраняется в JSON проекта.
- Solution Explorer больше не использует чекбоксы: двойной клик открывает файл через ОС; папки и файлы отображаются иерархически.
- Для панели контекста добавлены GridSplitter и скрытие до выбора проекта.
