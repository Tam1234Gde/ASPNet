"""Sequential per-instrument analysis with account-wide reconciliation."""
import asyncio
from dataclasses import replace
from .control import Control
from .runtime import event

async def multi_cycle(s, ai, broker, cycle_fn, reconcile_fn):
    c = Control()
    for uid in s.instrument_uids:
        if c.get('paused', True):
            break
        if not c.instruments(s).get(uid, False):
            continue
        if await reconcile_fn(s, broker):
            break
        try:
            await cycle_fn(replace(s, instrument_uids=(uid,)), ai, broker)
        except ValueError as exc:
            # Only locally raised validation messages; no provider response text.
            event(s, 'INSTRUMENT_ERROR', {'instrument':uid, 'type':type(exc).__name__})
        # Unknown submission/network errors escape and stop the entire iteration.
        await asyncio.sleep(0.2)
        if await reconcile_fn(s, broker):
            break
