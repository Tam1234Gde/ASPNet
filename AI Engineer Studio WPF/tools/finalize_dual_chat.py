from pathlib import Path
p=Path('src/Presentation/MainWindow.xaml.cs');s=p.read_text(encoding='utf-8')
s=s.replace('    const int ProjectHistoryLoadLimit = 3;\n','').replace('    int loadedDevelopmentHistoryCount;\n','').replace('    int loadedConversationHistoryCount;\n','').replace('loadedDevelopmentHistoryCount=0; loadedConversationHistoryCount=0; ','')
s=s.replace('        if (target != activeChatType) return;', '        ModelPickerButton.IsEnabled = !isWaiting;\n        if (target != activeChatType) return;')
s=s.replace('        var text=PromptInput.Text.Trim();', '        string text=PromptInput.Text.Trim();')
a=s.index('    async Task SendConversationAsync');b=s.index('    async Task<string> AskNeuralNetwork',a)
part=s[a:b]
for old,new in [('var started=', 'Stopwatch started='),('var answer=', 'string answer='),('var bridge=', 'string bridge='),('var system=', 'string system='),('var payload=', 'string payload='),('var psi=', 'ProcessStartInfo psi='),('using var process=', 'using Process process='),('var outputLines=', 'List<string> outputLines='),('var output=', 'string output='),('var error=', 'string error='),('using var doc=', 'using JsonDocument doc='),('out var eventKind','out JsonElement eventKind'),('out var ok','out JsonElement ok')]:part=part.replace(old,new)
part=part.replace('        await process.StandardInput.WriteAsync(payload);', '        Task<string> errorTask = process.StandardError.ReadToEndAsync();\n        await process.StandardInput.WriteAsync(payload);').replace('string error=await process.StandardError.ReadToEndAsync();','string error=await errorTask;')
s=s[:a]+part+s[b:]
p.write_text(s,encoding='utf-8')
p=Path('versions');s=p.read_text(encoding='utf-8-sig');p.write_text(s+'\n\n2026-02-21 · 1.0.20260221.1\n- Завершены чаты «Разработка» и «Общение»: независимые запросы, черновики, сохранение обеих историй.\n- Общий контекст двунаправленный: последние 24 сообщения другого чата, до 24000 символов; история хранится полностью.\n- Общение выполняет один API-запрос без инструментов и этапа планирования.\n- Исправлена передача контекста в Python bridge. Старые сообщения считаются разработкой.\n- Новый запрос не удаляет историю. Резервная копия: backups/2026-02-21-chat-modes.\n',encoding='utf-8')
