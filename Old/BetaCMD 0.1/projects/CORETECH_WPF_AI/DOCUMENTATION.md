# CORETECH IT DEV - Полная Документация

## 🚀 Обзор
CORETECH IT DEV - это современное WPF-приложение для взаимодействия с AI-ассистентами в разработке. Приложение поддерживает множество моделей (Claude, GPT, DeepSeek, Grok) и имеет элегантный фиолетовый интерфейс.

## ✨ Основные Возможности

### 🎨 Интерфейс
- **Фиолетовая тема** с акцентными цветами (#8B5CF6, #A78BFA, #C084FC)
- **Закругленные углы** на всех элементах (8-12px border radius)
- **Адаптивное поле ввода** - многострочное, с авто-прокруткой, min 80px / max 200px
- **Современная типографика** - увеличенные шрифты, высокая читаемость

### ⚙️ Управление Настройками
- **Панель настроек** с overlay эффектом
- **Редактирование API ключей**: CVC API Key, OpenAI API Key
- **Настройка Base URL** для API
- **Сохранение в переменные окружения** (User + Process scope)

### 📁 Управление Проектом
- **Автообновление файлов** через FileSystemWatcher
- **Выбор папки проекта** - "📁 Choose Project Folder"
- **Добавление файлов** - "➕ Add Files to Project"
- **Список файлов** с hover-эффектами и выделением выбранных

### 🤖 AI Интеграция
- **Множество моделей**: claude-sonnet-4-6, gpt-5.6-terra, gpt-5.6-sol, deepseek-v4-flash, grok-4.5, claude-opus-5, qwen3.7-max
- **Потоковая передача ответов** (streaming)
- **История сообщений** с красивым отображением
- **Применение файловых директив** - кнопка "✅ Apply File Directives"

## 🔧 Технический Стек

### Основные технологии
- **.NET 10.0** (Windows)
- **WPF** (Windows Presentation Foundation)
- **Windows Forms** (для диалогов выбора файлов/папок)

### NuGet пакеты
- **CommunityToolkit.Mvvm 8.4.0** - MVVM framework
- **Microsoft.Extensions.DependencyInjection 10.0.0** - DI контейнер
- **Microsoft.Extensions.Http.Resilience 10.0.0** - HTTP resilience (retry, circuit breaker)
- **AvalonEdit 6.3.1.120** - редактор кода
- **Markdig 0.40.0** - Markdown парсер

### Архитектурные Паттерны
- **MVVM** (Model-View-ViewModel)
- **Dependency Injection**
- **Repository Pattern**
- **Async/Await** для всех I/O операций

## 📂 Структура Проекта

```
CORETECH_WPF_AI/
├── AISoftCraftDuck/
│   ├── App.xaml                     # Стили и ресурсы приложения
│   ├── App.xaml.cs                  # Точка входа, DI конфигурация
│   ├── MainWindow.xaml              # Главное окно UI
│   ├── MainWindow.xaml.cs           # Code-behind главного окна
│   ├── GlobalUsings.cs              # Глобальные using директивы
│   ├── CORETECH_IT_DEV.csproj       # Файл проекта
│   │
│   ├── Controls/
│   │   ├── CodeBlockControl.xaml   # Контрол для отображения кода
│   │   └── CodeBlockControl.xaml.cs
│   │
│   ├── Converters/
│   │   └── Converters.cs            # BoolToVisibility, InverseBool
│   │
│   ├── Models/
│   │   └── ChatModels.cs            # ChatMessage, DuckSettings, FileDirective
│   │
│   ├── Services/
│   │   ├── ApiDuckClient.cs         # HTTP клиент для AI API
│   │   ├── EnvService.cs            # Управление настройками и env переменными
│   │   ├── ProcessBridge.cs         # Запуск внешних процессов
│   │   └── ProjectServices.cs       # FileDirectiveService, DockerService
│   │
│   └── ViewModels/
│       └── MainViewModel.cs         # Главная ViewModel с бизнес-логикой
│
├── CORETECH_IT_DEV.sln              # Solution файл
├── build.bat                         # Скрипт сборки
├── run.bat                           # Скрипт запуска
└── README.md                         # Эта документация
```

## 🛠️ Установка и Запуск

### Предварительные требования
- Windows 10/11
- .NET 10.0 SDK или выше
- Visual Studio 2022 (опционально)

### Способ 1: Используя bat файлы

**Сборка проекта:**
```bash
build.bat
```

**Запуск приложения:**
```bash
run.bat
```

### Способ 2: Используя dotnet CLI

**Восстановление зависимостей:**
```bash
cd AISoftCraftDuck
dotnet restore
```

**Сборка:**
```bash
dotnet build --configuration Release
```

**Запуск:**
```bash
dotnet run
```

### Способ 3: Visual Studio
1. Откройте `CORETECH_IT_DEV.sln`
2. Нажмите F5 для запуска с отладкой
3. Или Ctrl+F5 для запуска без отладки

## ⚙️ Первоначальная Настройка

### 1. Запустите приложение
После первого запуска вы увидите главное окно с фиолетовой темой.

### 2. Откройте настройки
Нажмите кнопку **"⚙️ Settings"** в левой панели.

### 3. Заполните настройки

**API Base URL** (по умолчанию):
```
https://apiduck.sytes.net/v1
```

**CVC API Key**:
- Введите ваш API ключ от CVC

**OpenAI API Key**:
- Введите ваш OpenAI API ключ (если используете OpenAI совместимые модели)

### 4. Сохраните настройки
Нажмите **"Save"** - настройки сохранятся в переменные окружения Windows.

## 💡 Использование

### Выбор проекта
1. Нажмите **"📁 Choose Project Folder"**
2. Выберите папку с вашим проектом
3. Список файлов автоматически обновится

### Добавление файлов
1. Нажмите **"➕ Add Files to Project"**
2. Выберите один или несколько файлов
3. Файлы будут скопированы в папку проекта

### Работа с AI
1. Выберите модель из выпадающего списка
2. Введите ваш запрос в поле ввода (поддерживается многострочный ввод)
3. Нажмите **"Send 🚀"** или Enter
4. Ответ будет отображаться в реальном времени (streaming)

### Применение файловых директив
Если AI вернул ответ с директивами вида:
```
### FILE: src/main.js
console.log('Hello World');

### FILE: src/utils.js
export const add = (a, b) => a + b;
```

Нажмите кнопку **"✅ Apply File Directives"** под сообщением, и файлы будут автоматически созданы/обновлены в вашем проекте.

## 🔍 Решение Проблем

### ❌ Ошибка 401 Unauthorized
**Причина**: API ключ не настроен или неверный.

**Решение**:
1. Откройте Settings (⚙️)
2. Убедитесь, что API Key или OpenAI API Key заполнен
3. Проверьте, что Base URL правильный
4. Нажмите Save
5. Попробуйте отправить сообщение снова

### ❌ Файлы не обновляются
**Решение**:
1. Попробуйте снова выбрать папку проекта через "📁 Choose Project Folder"
2. Проверьте, что у приложения есть права на чтение папки
3. FileSystemWatcher должен автоматически отслеживать изменения

### ❌ Ошибки компиляции
**Решение**:
```bash
cd AISoftCraftDuck
dotnet clean
dotnet restore
dotnet build
```

### ❌ Приложение не запускается
**Проверьте**:
- Установлен ли .NET 10.0 SDK: `dotnet --version`
- Нет ли ошибок в логах сборки
- Все ли NuGet пакеты восстановлены

## 🎨 Кастомизация

### Изменение цветовой схемы
Откройте `App.xaml` и измените цвета в секции Resources:

```xml
<SolidColorBrush x:Key="PrimaryPurple" Color="#8B5CF6"/>
<SolidColorBrush x:Key="PrimaryPurpleHover" Color="#A78BFA"/>
<SolidColorBrush x:Key="AccentPurple" Color="#C084FC"/>
```

### Добавление новых моделей
Откройте `ViewModels/MainViewModel.cs` и добавьте модель в массив:

```csharp
public string[] Models { get; } = [
    "claude-sonnet-4-6", 
    "gpt-5.6-terra", 
    "your-new-model"  // <-- добавьте здесь
];
```

## 🔐 Безопасность

### Хранение API ключей
- API ключи сохраняются в **переменные окружения Windows** (User scope)
- Ключи НЕ хранятся в plain text файлах
- При каждом запросе ключ читается из EnvService

### Безопасность файловых операций
- **Path traversal protection** - блокируется выход за пределы проекта
- Все файловые операции обернуты в try-catch
- FileSystemWatcher работает только внутри выбранной папки

## 🏗️ Архитектура

### Dependency Injection
Все сервисы регистрируются в `App.xaml.cs`:

```csharp
services.AddSingleton<IEnvService, EnvService>();
services.AddSingleton<FileDirectiveService>();
services.AddHttpClient<IApiDuckClient, ApiDuckClient>()
    .ConfigureHttpClient(...)
    .AddStandardResilienceHandler(...);
```

### HTTP Resilience
Используется `Microsoft.Extensions.Http.Resilience` с настройками:
- **Retry**: 2 попытки
- **Backoff**: Exponential с jitter
- **Timeout**: 5 минут

### Async Streaming
API клиент использует `IAsyncEnumerable<string>` для потоковой передачи:

```csharp
await foreach (var token in api.StreamAsync(history, model))
{
    answer.Content += token;
}
```

## 📊 Производительность

### Оптимизации
- **Lazy loading** файлов (до 500 файлов)
- **Debouncing** для FileSystemWatcher
- **Async/await** для всех I/O операций
- **HttpClient pooling** через IHttpClientFactory

### Ограничения
- Максимум 500 файлов в списке
- Timeout для HTTP запросов: 5 минут
- Retry попыток: 2

## 📝 Лицензия
MIT

## 👥 Поддержка
При возникновении проблем:
1. Проверьте раздел "Решение Проблем"
2. Убедитесь, что все зависимости установлены
3. Проверьте логи в консоли

## 🔄 Обновления
- v1.0.0 - Первый релиз с фиолетовой темой
- Автообновление файлов
- Панель настроек API
- Исправлена проблема 401 Unauthorized

---

**Создано с ❤️ для разработчиков**
