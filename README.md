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
```
### install for ClassifiedConsumer
```
python -m pip install confluent-kafka
python -m pip install redis
```