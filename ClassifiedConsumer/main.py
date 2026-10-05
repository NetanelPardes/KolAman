import json
import redis
from confluent_kafka import Consumer, KafkaException
from Validation import is_valid

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

        saved = redis_client.set(
            event_id,
            json.dumps(alert, ensure_ascii=False),
            nx=True,
            ex=300
        )

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
