# CORETECH IT DEV

AI-powered development assistant with modern purple-themed interface.

## Features

✨ **Modern UI** - Purple-themed interface with rounded corners and smooth animations
🔄 **Auto-refresh** - File browser automatically updates when project files change
⚙️ **Settings Panel** - Easy configuration of API keys and base URL
📁 **Project Management** - Choose project folders and add files easily
🤖 **Multiple AI Models** - Support for Claude, GPT, DeepSeek, Grok, and more
💬 **Streaming Responses** - Real-time AI responses with token streaming
📝 **File Directives** - Apply `### FILE:` directives directly to your project

## Setup

1. **Build the project:**
   ```bash
   cd AISoftCraftDuck
   dotnet build
   ```

2. **Run the application:**
   ```bash
   dotnet run
   ```

3. **Configure API settings:**
   - Click the "⚙️ Settings" button
   - Enter your API Base URL (default: `https://apiduck.sytes.net/v1`)
   - Enter your API Key or OpenAI API Key
   - Click "Save"

## Usage

1. **Select a project folder** using "📁 Choose Project Folder" button
2. **Add files** to your project with "➕ Add Files to Project"
3. **Choose an AI model** from the dropdown
4. **Type your message** in the input field (supports multi-line)
5. **Click "Send 🚀"** to get AI assistance
6. **Apply file directives** using the "✅ Apply File Directives" button in responses

## Requirements

- .NET 10.0 or higher
- Windows OS (WPF application)

## Architecture

- **MVVM Pattern** with CommunityToolkit.Mvvm
- **Dependency Injection** with Microsoft.Extensions.DependencyInjection
- **HTTP Resilience** with retry policies and circuit breakers
- **Real-time file monitoring** with FileSystemWatcher
- **Async streaming** for AI responses

## Troubleshooting

### 401 Unauthorized Error
- Open Settings and verify your API key is correctly entered
- Make sure the Base URL is correct
- Check that your API key has the necessary permissions

### Files not updating
- The file watcher should update automatically
- Try clicking "📁 Choose Project Folder" again to refresh

### Build errors
- Run `dotnet restore` to restore NuGet packages
- Make sure you're using .NET 10.0 SDK

## License

MIT
