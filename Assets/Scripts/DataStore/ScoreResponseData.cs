using System.Text.Json.Serialization;

namespace DataStore
{
        [System.Serializable]
        public class ScoreResponseData 
        {
            [JsonPropertyName("value")]
            public float Value { get; set; }
            [JsonPropertyName("player_name")]
            public string PlayerName { get; set; }
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("created_at")]
            public string CreatedAt { get; set; }
            [JsonPropertyName("updated_at")]
            public string UpdatedAt { get; set; }
        }
}