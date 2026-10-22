# Незакрытые проблемы и технический долг

Дата обновления: 2026-09-22.

## Критичные

### 1. Сборка требует повторной проверки после перехода на `OpenFolderDialog`
Правка заменила обходной `OpenFileDialog` на нативный `OpenFolderDialog`, однако последняя команда сборки в автоматизированной среде завершилась некорректно из-за путей с пробелами/ограничений оболочки. Нужна локальная проверка:

```powershell
cd "AI Engineer Studio WPF"
dotnet build AIEngineerStudio.Wpf.csproj
```

Если `OpenFolderDialog` отсутствует в SDK, заменить на COM `IFileOpenDialog` или подключить Windows API Code Pack.

### 2. Tool calling не реализован полностью
Bridge выполняет только chat completion. Инструменты из `Логика работы с нейросетью.py` (`read_file`, `write_file`, `search_code`, `execute_command` и другие) не перенесены.

**Нужно:** безопасный tools loop, ограничение рабочей папкой, approval UI, отображение tool events в inspector.

## Высокий приоритет

### 3. Путь Python есть в модели, но отсутствует в окне настроек
`AppSettings.PythonPath` и `MainWindow.PythonPath` добавлены, но `SettingsWindow` пока не содержит поле выбора пути.

### 4. Нет отмены генерации
`CancellationTokenSource` объявлен/запланирован, но процесс Python и HTTP-запрос не отменяются кнопкой Stop.

### 5. API-key обрабатывается в bridge через JSON stdin
DPAPI защищает токен на диске, но во время запроса токен доступен дочернему Python-процессу. Это штатно для текущей архитектуры, но не оптимально.

**Нужно:** при необходимости перейти на нативный .NET-клиент OpenAI-compatible API или защищённый локальный service boundary.

## Средний приоритет

### 6. Markdown renderer ограничен
Есть заголовки `##`, bold, простые списки и fenced code. Нет ссылок, таблиц, nested lists, syntax highlighting, HTML sanitization.

### 7. Preview демонстрационный
Live Preview показывает карточку и может открыть URL браузером, но не содержит WebView2, изолированной sandbox и lifecycle dev server/HMR.

### 8. Solution Explorer не полностью интерактивен
Нет FileSystemWatcher, lazy loading, drag-drop, контекстных команд (rename/delete/new), вкладок открытых редакторов и сохранения состояния раскрытых папок.

### 9. История сообщений не сжимается
Ограничен размер выбранного контекста, но история диалога может расти неограниченно.

### 10. Темы меняют не все статические цвета
Большая часть UI использует DynamicResource; demo preview и некоторые цвета Markdown/диалогов всё ещё заданы напрямую.

## Низкий приоритет

### 11. Нет тестов
Нужны unit tests для SettingsStore, Markdown, контекста и bridge contract; integration tests для API mock.

### 12. Нет packaging/installer
Нет self-contained publish/MSIX, bootstrap для Python/openai и диагностики зависимостей.

### 13. Локализация неполная
Интерфейс преимущественно русский, но технические статусы и названия моделей смешанные.

### 14. Возможен nullable warning
После успешной локальной сборки нужно устранить оставшиеся nullable warnings, если они появятся.

## Исправлено после первого реестра

- Настройки сохраняются в `%AppData%\AIEngineerStudio\settings.json`.
- API-токен защищается DPAPI.
- Добавлены Graphite/Светлая/Midnight Blue/Emerald Night темы.
- Добавлена кнопка `☀` быстрой смены темы и `⚙` настроек.
- Добавлены поля токена, Base URL, модели, temperature, max tokens, темы и системного промпта.
- File picker заменён на `OpenFolderDialog` (требует финальной build-проверки).
- Кнопки получили собственный шаблон без стандартной синей WPF-подсветки.

## Результат рефакторинга

Вынесены:
- DTO в `Models/ChatModels.cs`;
- хранение настроек в `Services/SettingsStore.cs`;
- файловые операции и контекст в `Services/ProjectFileService.cs`;
- запуск Python/API bridge в `Services/NeuralBridgeService.cs`;
- Markdown и темы в `UI/MarkdownRenderer.cs` и `UI/ThemeManager.cs`;
- диалоги в `Views/Dialogs.cs`.

Текущая сборка после рефакторинга: 0 ошибок, 2 предупреждения (nullable и неиспользуемый `generationCts`).

## После перехода на модель Project Document
- Реальное выполнение file-write/terminal tool calls ещё требует approval loop.
- История сохраняется после ответа ассистента; желательно сохранять также изменения описания проекта и промежуточное состояние запроса.
- Требуется добавить команду «Сменить проект» в Activity Bar или меню проекта; базовый стартовый выбор уже реализован.
- Необходимо убрать nullable warning `ProjectStore.Names` и неиспользуемый `generationCts`.
