$ErrorActionPreference = 'Stop'
$outputPath = Join-Path $PSScriptRoot 'AI_Agent_Improvement_Options.docx'
$word = New-Object -ComObject Word.Application
$word.Visible = $false
try {
    $document = $word.Documents.Add()
    $selection = $word.Selection

    $selection.Style = 'Title'
    $selection.TypeText('Варианты улучшения функционала AI-агента')
    $selection.TypeParagraph()
    $selection.Style = 'Normal'
    $selection.TypeText('Проект: CORETECH_WPF_AI')
    $selection.TypeParagraph()
    $selection.TypeParagraph()

    function Add-Heading([string]$text) {
        $script:selection.Style = 'Heading 1'
        $script:selection.TypeText($text)
        $script:selection.TypeParagraph()
        $script:selection.Style = 'Normal'
    }
    function Add-Item([string]$title, [string]$text) {
        $script:selection.Font.Bold = 1
        $script:selection.TypeText($title + ': ')
        $script:selection.Font.Bold = 0
        $script:selection.TypeText($text)
        $script:selection.TypeParagraph()
    }

    $selection.TypeText('WPF-клиент и CLI-прототип agent.py изучены. Сборка dotnet build проходит успешно, но есть предупреждения: конфликт Language в CodeBlockControl и использование EndOfStream в асинхронном SSE-чтении.')
    $selection.TypeParagraph()
    $selection.TypeParagraph()

    Add-Heading 'Критичные улучшения'
    Add-Item '1. Отозвать API-ключи' 'Ключи находятся в .env, имени отдельного файла и fallback-значении agent.py. Их следует отозвать, удалить из репозитория, добавить .gitignore и хранить через Windows Credential Manager или DPAPI.'
    Add-Item '2. Закрыть path traversal' 'FileDirectiveService.WriteAsync не проверяет, что результирующий путь остаётся внутри выбранного проекта. Модель может передать ../../. Нужны канонизация и проверка пути.'
    Add-Item '3. Не применять изменения автоматически' 'Сейчас любой ответ с блоком filename.ext сразу перезаписывает файл. Нужны preview/diff, подтверждение, резервная копия, отмена и rollback.'
    Add-Item '4. Реализовать настоящий tool-calling в WPF' 'CLI уже поддерживает Function Calling, а WPF извлекает файлы из Markdown. Следует добавить цикл вызовов get_tree, read_file, search, write, edit и run_command с возвратом результатов модели.'

    Add-Heading 'Функциональные улучшения'
    Add-Item '5. Контекст проекта' 'Добавить поиск по коду, чтение диапазонов строк, дерево проекта с исключениями bin, obj, .git и node_modules, а также лимиты размера и выбор релевантных файлов.'
    Add-Item '6. Редактирование патчами' 'Вместо полной перезаписи файлов использовать edit_file_part или unified diff. Это уменьшит риск потери существующего кода.'
    Add-Item '7. Терминал и проверка результата' 'Подключить существующий IProcessBridge в сценарий агента для build, test, lint и git status. Использовать allowlist, таймауты, логи и подтверждение опасных команд.'
    Add-Item '8. Планирование задач' 'Добавить режим: анализ, план, подтверждение, выполнение шагов. В UI показывать прогресс, вызовы инструментов, изменённые файлы и результаты тестов.'
    Add-Item '9. Отмена запроса' 'ApiDuckClient поддерживает CancellationToken, но UI его не использует. Нужны кнопка Stop и CancellationTokenSource на активный запрос.'
    Add-Item '10. Контекст и память' 'Ограничивать историю по токенам, автоматически резюмировать старые сообщения и хранить отдельную память проекта: архитектуру, соглашения и решения.'

    Add-Heading 'Надёжность и UX'
    Add-Item '11. Настройки API' 'HttpClient получает BaseAddress при создании. После сохранения нового BaseUrl изменения могут не примениться без перезапуска. Адрес и proxy нужно обновлять для нового запроса или пересоздавать клиент.'
    Add-Item '12. Git' 'Автокоммиты каждые 30 секунд могут фиксировать нерабочий код. Лучше создать отдельную ветку агента и выполнять commit после успешных проверок, с просмотром diff и ручным merge/rollback.'
    Add-Item '13. FileSystemWatcher' 'Добавить debounce и исключения bin, obj, .git, чтобы не обновлять список файлов многократно во время сборки.'
    Add-Item '14. Модели и провайдеры' 'Вынести список моделей из кода в конфигурацию, проверять доступность модели, добавить temperature, max tokens и профили для анализа, кода и быстрых исправлений.'
    Add-Item '15. Тестирование и архитектура' 'Добавить unit-тесты для SSE-парсинга, .env, безопасных путей, директив и Git. Вынести файловую логику, ошибки и логи из MainViewModel в сервисы.'

    Add-Heading 'Приоритет реализации'
    Add-Item '1' 'Удаление и ротация ключей, защита путей.'
    Add-Item '2' 'Preview/diff и подтверждение изменений.'
    Add-Item '3' 'Настоящий tool-calling в WPF.'
    Add-Item '4' 'Build/test loop и отмена операций.'
    Add-Item '5' 'Управление контекстом, Git-ветки и тестовое покрытие.'

    $document.SaveAs([ref]$outputPath, [ref]16)
    $document.Close()
}
finally {
    $word.Quit()
    [void][System.Runtime.InteropServices.Marshal]::ReleaseComObject($word)
}
