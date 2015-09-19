using System;
using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Objects
{
    public class AnimeDetailsJson : IAnimeDetailsJson
    {
        [JsonProperty(PropertyName = "anime_id")]
        public int AnimeId { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public int Episodes { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }

        [JsonProperty(PropertyName = "downloaded_episodes")]
        public string DownloadedEpisodes { get; set; }

        [JsonProperty(PropertyName = "storage_type")]
        public int StorageType { get; set; }

        [JsonProperty(PropertyName = "storage_value")]
        public float StorageValue { get; set; }

        [JsonProperty(PropertyName = "rewatched")]
        public int Rewatched { get; set; }

        [JsonProperty(PropertyName = "rewatch_value")]
        public int RewatchValue { get; set; }

        [JsonProperty(PropertyName = "date_start")]
        public string DateStart { get; set; }

        [JsonProperty(PropertyName = "date_finished")]
        public string DateFinish { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }

        [JsonProperty(PropertyName = "enable_discussion")]
        public int EnableDiscussion { get; set; }

        [JsonProperty(PropertyName = "enable_rewatching")]
        public int EnableRewatching { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "fansub_group")]
        public string FansubGroup { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }
    }
}