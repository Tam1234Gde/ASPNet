"""Offline regression tests: no credentials or network required."""
import contextlib
import io
import json
from pathlib import Path
import runpy
import sys
import types
import unittest

class BridgeTests(unittest.TestCase):
    def invoke(self, mode):
        calls = []
        def create(**kwargs):
            calls.append(kwargs)
            message = types.SimpleNamespace(content='test answer', tool_calls=None)
            return types.SimpleNamespace(choices=[types.SimpleNamespace(message=message)])
        fake = types.ModuleType('openai')
        fake.OpenAI = lambda **kwargs: types.SimpleNamespace(chat=types.SimpleNamespace(completions=types.SimpleNamespace(create=create)))
        original = sys.modules.get('openai')
        original_input = sys.stdin
        sys.modules['openai'] = fake
        sys.stdin = io.StringIO(json.dumps({'api_key':'offline', 'model':'test', 'chat_mode':mode, 'context':'SHARED_CONTEXT', 'history':[{'role':'user','content':'hello'}]}))
        try:
            output = io.StringIO()
            with contextlib.redirect_stdout(output):
                runpy.run_path(str(Path(__file__).with_name('neural_bridge.py')))
            return calls, [json.loads(line) for line in output.getvalue().splitlines()]
        finally:
            sys.stdin = original_input
            if original is None:
                sys.modules.pop('openai', None)
            else:
                sys.modules['openai'] = original

    def test_conversation_no_tools_no_plan(self):
        calls, events = self.invoke('conversation')
        self.assertEqual(len(calls), 1)
        self.assertNotIn('tools', calls[0])
        self.assertEqual([event['event'] for event in events], ['final'])
        self.assertTrue(any(m['content'] == 'SHARED_CONTEXT' for m in calls[0]['messages']))

    def test_development_keeps_tools_and_shared_context(self):
        calls, events = self.invoke('development')
        self.assertEqual(len(calls), 2)
        self.assertTrue(calls[1]['tools'])
        self.assertEqual([event['event'] for event in events], ['plan', 'final'])
        self.assertTrue(any(m['content'] == 'SHARED_CONTEXT' for m in calls[1]['messages']))

if __name__ == '__main__':
    unittest.main()
