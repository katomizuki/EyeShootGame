
namespace DataStore
{
        [System.Serializable]
        public class ScoreResponseData 
        {
            public float value { get; set; }
            public string player_name { get; set; }
            public int id { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }
}