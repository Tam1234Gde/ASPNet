# Руководство разработки

## Перед изменением
1. Прочитать `PROJECT_CONTEXT.md`.
2. Прочитать `KNOWN_ISSUES.md`.
3. Проверить, не добавляет ли изменение секреты, системные WPF-стили или небезопасные file/terminal tools.

## Проверка
Запускать из каталога `AI Engineer Studio WPF`:

```powershell
python -m py_compile tools/neural_bridge.py
dotnet build AIEngineerStudio.Wpf.csproj
```

## Соглашения
- UI-цвета — через `DynamicResource`.
- Ключи не хранить в исходниках.
- Настройки менять через `SettingsStore`, токен не писать открытым текстом.
- Запросы к модели идут через `neural_bridge.py` до полной реализации native .NET клиента.
- Реальные `write_file`/terminal actions требуют approval.
- Не редактировать bin/obj/.vs/__pycache__.
- Перед завершением задачи обновлять `PROJECT_CONTEXT.md`, `README.md` и `KNOWN_ISSUES.md`.
