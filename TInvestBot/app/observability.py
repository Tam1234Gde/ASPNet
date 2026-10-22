"""Unified console and file logging for operations."""
import logging
from logging.handlers import RotatingFileHandler
from pathlib import Path

def setup_logging():
    Path('state').mkdir(exist_ok=True)
    logging.basicConfig(level=logging.INFO,
        format='%(asctime)s %(levelname)s %(name)s %(message)s',
        handlers=[logging.StreamHandler(), RotatingFileHandler('state/bot.log', maxBytes=5000000, backupCount=5, encoding='utf-8')],
        force=True)
