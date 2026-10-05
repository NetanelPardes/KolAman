from datetime import datetime
import logging

logger = logging.getLogger(__name__)

def is_valid(alert):

    if not isinstance(alert, dict):
        return False

    fields = ["alert_id","source","title","content","priority","classification","timestamp","status"]

    for field in fields:
        if field not in alert:
            print(f"{field} is missing")
            return False
        
        value = alert.get(field)

        if not isinstance(value, str) or not value.strip():
            return False

    if alert["source"] not in ["aman","mossad","pikud-haoref","shabak"]:
        print("source is: " + alert["source"])
        logger.error(f"alert {alert['alert_id']} hava a problem with the source")
        return False

    if alert["priority"] not in ["LOW","MEDIUM","HIGH","CRITICAL"]:
        print("priority is: " + alert["priority"])
        return False

    if alert["classification"] not in ["UNCLASSIFIED","RESTRICTED","SECRET","TOP_SECRET"]:
        print("classification is: " + alert["classification"])
        return False

    if alert["status"] != "WAITING":
        print("status is: " + alert["status"])
        return False

    lat = alert.get("lat")
    lon = alert.get("lon")

    if 'lat' not in alert:
            print("lat is missing")
            return False
    if type(lat) not in (int, float):
        print("lat is: " + str(alert["lat"]))
        return False

    if 'lon' not in alert:
        print("lon is missing")
        return False
    if type(lon) not in (int, float):
        print("lon is: " + str(alert["lon"]))
        return False

    return True