# KolAman

## NotificationGate
```
בשביל הפרודיוסר ששולח לקאפקא השתמשתי בבדיקה האם קיים הקובץ, ready
במידה והוא קיים אז קיים גם הקובץ גייסון
אז השתמשתי בפונקציה ריפלייס להחליף את הסיומתת המרתי את הטקסט שנקרא ממנו לגייסון, ושלחתי אותו לקאפקא
```

### install for NotificationGate
```bash
dotnet add package Confluent.Kafka
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Serilog.Sinks.File
dotnet add package Serilo
```

## ClassifiedConsumer
```
השתמשתי פה בקונסיומר פשוט של קאפקא
קורא את ההודדעות של קאפקא
אם הם סטרינג אני ממיר אותם לגייסון
ואז אני מכניס אותם לרדיס עם TTL של 300 שניות
הפונקציה שמכניסה לרדיס היא בודקת אם הוא קיים, אם הוא קיים היא תכניס
אם לא היא פשוט תתעלם
ובחרתי דווקא להתעלם, כיוון שזה 5 דקות

בקובץ ולידציות:
קודם כל החלטתי לבדוק האם כל השדות קיימים ולבדוק שיש לכולם ערכים
אחר כך החלטתי לבדוק האם המקור הוא מוסד שבכ אמן או פיקוד העורף
אחר כך החלטי לבדוק את העדיפות
אחר כך החלטתי לבדוק את סיווג של ההתראה
החלטתי לבדוק שהסיווג הוא warnng
והחלטתי לבדוק שהקווי אורך ורוחב הם מספרים 
```
### install for ClassifiedConsumer
```
python -m pip install confluent-kafka
python -m pip install redis
python -m pip install pika
python -m pip install shapely 
python -m pip install geopandas
```

### install for evry pikuf
```
dotnet add package RabbitMQ.Client --version 7.1.2
dotnet add package MongoDB.Driver --version 3.12.0
```
