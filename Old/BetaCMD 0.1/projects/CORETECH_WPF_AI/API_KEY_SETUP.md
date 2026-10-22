# CORETECH IT DEV - API Configuration Guide

## Your API Key
```
sk-cvc-8f869cae5afbadb7f7b8f3197733541084bfe93f146a64a1fde9e4cc3a636eed
```

## Quick Setup

### Option 1: Using the Application UI (Easiest)

1. Run the application
2. Click **⚙️ Settings** button
3. Enter your API key in the **API Key (Your CVC Token)** field
4. Click **Save**
5. You're ready to go!

### Option 2: Edit .env File Manually

Create or edit the `.env` file in the root directory:

```env
ANTHROPIC_BASE_URL="https://apiduck.sytes.net"
ANTHROPIC_AUTH_TOKEN="sk-cvc-8f869cae5afbadb7f7b8f3197733541084bfe93f146a64a1fde9e4cc3a636eed"
ANTHROPIC_MODEL="claude-sonnet-4-6"
```

Save and restart the application.

## Settings Explained

### API Base URL
```
https://apiduck.sytes.net/v1
```
This is the endpoint for the API Duck service. Already pre-configured.

### API Key (CVC Token)
```
sk-cvc-8f869cae5afbadb7f7b8f3197733541084bfe93f146a64a1fde9e4cc3a636eed
```
Your personal access token for API Duck. This key gives you access to all available models.

### System Prompt (Advanced)
The system prompt defines how the AI behaves. Default:
```
Ты — автономный ИИ-разработчик. Ты работаешь внутри каталога проекта...
```

You can customize this to:
- Add coding standards for your team
- Specify preferred libraries and frameworks
- Define comment and documentation style

## Features

### Real-time File Monitoring
The AI shows exactly what it's doing in real-time:
```
🔧 Writing file: Services/NewService.cs
✅ Applied 3 file(s) successfully
📝 File Created: MainWindow.xaml
```

### Auto Git Commits
Every change made by AI is automatically:
- Tracked in Git
- Committed every 30 seconds
- Saved with timestamps
- Backed up with full history

Example commit message:
```
Auto-commit: AI changes at 2024-01-15 14:32:45
```

### Auto-scroll Chat
The chat automatically scrolls to show new messages. Scroll up to read history - auto-scroll will pause. Scroll to bottom to re-enable.

### Available Models
- **claude-sonnet-4-6** - Best for code writing and refactoring
- **gpt-5.6-terra** - Universal flagship model
- **gpt-5.6-luna** - Fast for simple tasks
- **deepseek-v4-flash** - Ultra-fast code generation
- **deepseek-v4-pro** - Mathematical calculations
- **grok-4.5** - Creative solutions
- **claude-opus-4-6** - Deep reasoning and documentation
- **qwen3.7-max** - JSON and structured data
- **kimi-k3** - Large context processing

## Building and Running

### Build the project:
```bash
build.bat
```

### Run the application:
```bash
run.bat
```

Or directly:
```bash
cd AISoftCraftDuck\bin\Debug\net10.0-windows
CORETECH_IT_DEV.exe
```

## Troubleshooting

### "API Key is not configured"
- Open Settings and enter your key
- Verify the .env file contains your key
- Restart the application

### ComboBox text not visible
Fixed! Model selection now has proper contrast.

### Settings window cut off
Fixed! Settings window now has scrolling and proper height.

### No auto-scroll in chat
Fixed! Chat now auto-scrolls, and you can disable it by scrolling up manually.

### Git errors
Make sure Git is installed:
```bash
git --version
```

Download from: https://git-scm.com/

## How It Works

1. **You ask** - Type your request in the chat
2. **AI analyzes** - Reads your project files and structure
3. **AI works** - Creates/modifies files, shows progress in real-time
4. **Auto-apply** - Changes are automatically applied to your project
5. **Auto-commit** - Git tracks all changes automatically
6. **You verify** - Review changes in your IDE or Git history

## Example Usage

### Create a new feature
```
Add a dark theme toggle to the settings
```

### Fix bugs
```
The save button doesn't work in settings, please fix
```

### Refactor code
```
Refactor MainViewModel to use async/await properly
```

### Add tests
```
Create unit tests for the GitAutoCommitService
```

## Support

For issues or questions, contact CORETECH support.
