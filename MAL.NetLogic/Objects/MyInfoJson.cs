using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Objects
{
    public class MyInfoJson : IMyInfoJson
    {
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "watching")]
        public int Watching { get; set; }

        [JsonProperty(PropertyName = "completed")]
        public int Completed { get; set; }

        [JsonProperty(PropertyName = "on_hold")]
        public int OnHold { get; set; }

        [JsonProperty(PropertyName = "dropped")]
        public int Dropped { get; set; }

        [JsonProperty(PropertyName = "plan_to_watch")]
        public int PlanToWatch { get; set; }

        [JsonProperty(PropertyName = "days_watching")]
        public double DaysWatching { get; set; }
    }
}