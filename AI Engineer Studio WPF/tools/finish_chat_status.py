from pathlib import Path
p=Path(__file__).resolve().parents[1]/'src/Presentation/MainWindow.xaml.cs'
s=p.read_text(encoding='utf-8')
s=s.replace('readonly Dictionary<ChatType, string> chatDrafts = new();','readonly Dictionary<ChatType, string> chatDrafts = new();\n    readonly Dictionary<ChatType, string> chatStatuses = new();')
a=s.index('    async void Send()'); b=s.index('    async Task<string> AskConversationAsync()',a)
part=s[a:b]
import re
part=re.sub(r'StatusText.Text\s*=\s*([^;]+);', r'SetChatStatus(\1);', part)
s=s[:a]+part+s[b:]
s=s.replace('chatDrafts.Clear(); PromptInput.Clear();','chatDrafts.Clear(); chatStatuses.Clear(); PromptInput.Clear();')
s=s.replace('UpdateChatButtons(); LoadProjectMessages(); RenderWaiting(busyChats.Contains(type));','UpdateChatButtons(); LoadProjectMessages(); RenderWaiting(busyChats.Contains(type));\n        StatusText.Text = chatStatuses.GetValueOrDefault(type, type == ChatType.Conversation ? "Общение · без инструментов" : "Разработка · инструменты проекта");')
s=s.replace('    string SharedContext(ChatType target)', '''    void SetChatStatus(string status)
    {
        ChatType target = responseChat.Value ?? activeChatType;
        chatStatuses[target] = status;
        if (target == activeChatType) StatusText.Text = status;
    }
    string SharedContext(ChatType target)''')
p.write_text(s,encoding='utf-8')
