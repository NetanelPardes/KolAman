using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace hamal.Models
{
    [BsonIgnoreExtraElements]
    public class Alert
    {
        [BsonId]
        public BsonValue Id { get; set; } = BsonNull.Value;
        [JsonPropertyName("alert_id")]
        public string AlertId { get; set; } = "";

        [JsonPropertyName("source")]
        public string Source { get; set; } = "";

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";

        [JsonPropertyName("priority")]
        public string Priority { get; set; } = "";

        [JsonPropertyName("classification")]
        public string Classification { get; set; } = "";

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("command")]
        public string Command { get; set; } = "";
        public double DurationSeconds { get; set; }
    }
}