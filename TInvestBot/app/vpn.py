"""Optional Xray/VLESS launcher used only as Telegram's local proxy."""
from __future__ import annotations
import json
import logging
import os
import shutil
import socket
import subprocess
import time
from pathlib import Path

CONFIG_PATH = Path(os.getenv("VPN_CONFIG_PATH", "state/telegram_xray.json"))


def validate_config(value: object) -> dict:
    if not isinstance(value, dict):
        raise ValueError("Конфигурация должна быть JSON-объектом")
    for key in ("inbounds", "outbounds"):
        if not isinstance(value.get(key), list) or not value[key]:
            raise ValueError(f"Отсутствует непустой раздел {key}")
    if not any(str(x.get("protocol", "")).lower() == "http" and int(x.get("port", 0)) > 0
               for x in value["inbounds"] if isinstance(x, dict)):
        raise ValueError("Нужен HTTP-inbound для Telegram (например 127.0.0.1:10809)")
    if not any(isinstance(x, dict) and str(x.get("protocol", "")).lower() == "vless"
               for x in value["outbounds"]):
        raise ValueError("В outbounds не найден VLESS")
    return value


def save_config(value: object) -> str:
    config = validate_config(value)
    CONFIG_PATH.parent.mkdir(parents=True, exist_ok=True)
    tmp = CONFIG_PATH.with_suffix(".tmp")
    tmp.write_text(json.dumps(config, ensure_ascii=False, indent=2), encoding="utf-8")
    tmp.replace(CONFIG_PATH)
    return str(CONFIG_PATH)


def local_http_proxy(value: dict) -> str:
    for inbound in value["inbounds"]:
        if isinstance(inbound, dict) and str(inbound.get("protocol", "")).lower() == "http":
            return f"http://{inbound.get('listen', '127.0.0.1')}:{int(inbound['port'])}"
    raise ValueError("HTTP-inbound не найден")


def load_config() -> dict | None:
    if not CONFIG_PATH.exists():
        return None
    return validate_config(json.loads(CONFIG_PATH.read_text(encoding="utf-8")))


def _executable() -> str:
    configured = os.getenv("XRAY_PATH", "xray").strip()
    found = shutil.which(configured) if not Path(configured).exists() else configured
    if not found:
        raise FileNotFoundError(f"xray.exe не найден: {configured}")
    return found


def _endpoint(proxy: str) -> tuple[str, int]:
    host_port = proxy.removeprefix("http://").removeprefix("https://").split("/", 1)[0]
    host, port = host_port.rsplit(":", 1)
    return host, int(port)


def start() -> tuple[subprocess.Popen | None, str | None]:
    config = load_config()
    if not config:
        logging.info("XRAY_CONFIG_NOT_FOUND path=%s", CONFIG_PATH)
        return None, None
    executable = _executable()
    # Проверяем конфигурацию самим Xray до запуска.
    check = subprocess.run([executable, "run", "-test", "-c", str(CONFIG_PATH)],
                           capture_output=True, text=True, timeout=15)
    if check.returncode:
        detail = (check.stderr or check.stdout).strip()[-500:]
        raise RuntimeError(f"Xray отклонил конфигурацию: {detail}")
    log_dir = Path("state")
    log_dir.mkdir(exist_ok=True)
    out = (log_dir / "xray.stdout.log").open("a", encoding="utf-8")
    err = (log_dir / "xray.stderr.log").open("a", encoding="utf-8")
    proc = subprocess.Popen([executable, "run", "-c", str(CONFIG_PATH)], stdout=out, stderr=err)
    proxy = local_http_proxy(config)
    host, port = _endpoint(proxy)
    deadline = time.monotonic() + 8
    while time.monotonic() < deadline:
        if proc.poll() is not None:
            raise RuntimeError("Xray завершился сразу после запуска; смотрите state/xray.stderr.log")
        try:
            with socket.create_connection((host, port), timeout=0.3):
                logging.info("XRAY_STARTED pid=%s proxy=%s config=%s", proc.pid, proxy, CONFIG_PATH)
                return proc, proxy
        except OSError:
            time.sleep(0.2)
    proc.terminate()
    raise RuntimeError(f"Xray не открыл локальный порт {host}:{port}")


def stop(proc: subprocess.Popen | None) -> None:
    if proc and proc.poll() is None:
        proc.terminate()
        try:
            proc.wait(timeout=5)
        except subprocess.TimeoutExpired:
            proc.kill()
