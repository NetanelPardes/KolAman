import json
import pika

connection = pika.BlockingConnection(
    pika.ConnectionParameters(
        host="localhost",
        port=5672
    )
)

channel = connection.channel()

channel.exchange_declare(
    exchange="alerts.exchange",
    exchange_type="direct",
    durable=True
)

for command in ["NORTH", "CENTER", "SOUTH", "OVERSEAS"]:
    queue = f"alerts.{command}"

    channel.queue_declare(queue=queue, durable=True)

    channel.queue_bind(
        exchange="alerts.exchange",
        queue=queue,
        routing_key=command
    )

channel.confirm_delivery()


def send_alert(alert, command):
    channel.basic_publish(
        exchange="alerts.exchange",
        routing_key=command,
        body=json.dumps(alert, ensure_ascii=False).encode("utf-8"),
        properties=pika.BasicProperties(delivery_mode=2),
        mandatory=True
    )

    print(f"Sent to Rabbit: {command}")


def close():
    connection.close()
