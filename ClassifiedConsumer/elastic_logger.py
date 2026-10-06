import requests
from datetime import datetime, timezone


def log_alert_sent(alert_id, command):
    log = {
        "@timestamp": datetime.now(timezone.utc).isoformat(),
        "alert_id": alert_id,
        "command": command,
        "event": "alert_sent"
    }

    try:
        response = requests.post("http://localhost:9200/kolaman-logs/_doc",json=log,timeout=5)
        response.raise_for_status()
    except requests.RequestException as error:
        print(f"Failed sending log to Elastic: {error}")