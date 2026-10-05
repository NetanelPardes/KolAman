from datetime import datetime


def is_valid(alert):

    if not isinstance(alert, dict):
        return False

    fields = ["alert_id","source","title","content","priority","classification","timestamp","status"]

    for field in fields:
        value = alert.get(field)

        if not isinstance(value, str) or not value.strip():
            return False

    if alert["source"] not in ["aman","mossad","pikud-haoref","shabak"]:
        print("source is: " + alert["source"])
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

    if type(lat) not in (int, float):
        print("lat is: " + alert["lat"])
        return False

    if type(lon) not in (int, float):
        print("lon is: " + alert["lon"])
        return False

    return True