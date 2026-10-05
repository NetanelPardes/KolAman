import json
import redis
from confluent_kafka import Consumer, KafkaException
from Validation import is_valid
from shapely.geometry import Point, Polygon
import geopandas as gpd
from shapely.geometry import Point
from rabbit_service import send_alert, close
import logging
from pathlib import Path

Path("Logs").mkdir(exist_ok=True)

logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s | %(levelname)s | %(message)s",
    datefmt="%Y-%m-%d %H:%M:%S",
    handlers=[
        logging.FileHandler(
            "Logs/Classified.log",
            encoding="utf-8"
        )
    ]
)
logger = logging.getLogger(__name__)

def get_region_with_geopandas(file_path: str, lon: float, lat: float) -> str:
    gdf = gpd.read_file(file_path)
    pt = Point(lon, lat)
    matched = gdf[gdf.geometry.contains(pt)]
    if not matched.empty:
        return matched.iloc[0]["region"]
    return "OVERSEAS"

# region = get_region_with_geopandas("regions.geojson", 34.800, 32.100)
# print(region)

config = {
    "bootstrap.servers": "localhost:9092",
    "group.id": "alert",
    "auto.offset.reset": "earliest",
    "enable.auto.commit": False
}

redis_client = redis.Redis(
    host="localhost",
    port=6379,
    decode_responses=True
)

consumer = Consumer(config)

topic = "alerts-events"
consumer.subscribe([topic])

try:
    redis_client.ping()

    print(f"Listening: {topic}")

    while True:
        message = consumer.poll(1.0)

        if message is None:
            continue

        if message.error():
            raise KafkaException(message.error())

        alert = json.loads(message.value().decode("utf-8"))

        if isinstance(alert, str):
            alert = json.loads(alert)

        if not is_valid(alert):
            print("Invalid alert")
            consumer.commit(message=message, asynchronous=False)
            continue
        event_id = alert.get("alert_id")

        command = get_region_with_geopandas("regions.geojson", alert['lon'], alert['lat'])

        alert["command"] = command
        send_alert(alert, command)
        consumer.commit(message=message, asynchronous=False)

        saved = redis_client.set(event_id,json.dumps(alert, ensure_ascii=False),nx=True,ex=300)

        if saved:
            print(f"Saved to Redis: {event_id}")
        else:
            print(f"Already exists: {event_id}")

        consumer.commit(message=message, asynchronous=False)

except KeyboardInterrupt:
    print("Processor stopped.")

finally:
    consumer.close()
    redis_client.close()
    close()

