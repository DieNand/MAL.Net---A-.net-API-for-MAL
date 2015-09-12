using System;
using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Objects
{
    public class ListAnimeJson : IListAnimeJson
    {
        [JsonProperty(PropertyName = "series_id")]
        public int SeriesId { get; set; }

        [JsonProperty(PropertyName = "series_title")]
        public string SeriesTitle { get; set; }

        [JsonProperty(PropertyName = "series_synonyms")]
        public string SeriesSynonyms { get; set; }

        [JsonProperty(PropertyName = "series_type")]
        public int SeriesType { get; set; }

        [JsonProperty(PropertyName = "series_episodes")]
        public int SeriesEpisodes { get; set; }

        [JsonProperty(PropertyName = "series_status")]
        public int SeriesStatus { get; set; }

        [JsonProperty(PropertyName = "series_start")]
        public string SeriesStart { get; set; }

        [JsonProperty(PropertyName = "series_end")]
        public string SeriesEnd { get; set; }

        [JsonProperty(PropertyName = "series_image")]
        public string SeriesImage { get; set; }

        [JsonProperty(PropertyName = "my_id")]
        public int MyId { get; set; }

        [JsonProperty(PropertyName = "watched_episodes")]
        public int WatchedEpisodes { get; set; }

        [JsonProperty(PropertyName = "my_start_date")]
        public string MyStartDate { get; set; }

        [JsonProperty(PropertyName = "my_finish_date")]
        public string MyFinishDate { get; set; }

        [JsonProperty(PropertyName = "my_score")]
        public int MyScore { get; set; }

        [JsonProperty(PropertyName = "my_status")]
        public int MyStatus { get; set; }

        [JsonProperty(PropertyName = "my_rewatching")]
        public string MyRewatching { get; set; }

        [JsonProperty(PropertyName = "my_rewatching_episodes")]
        public int RewatchingEpisode { get; set; }

        [JsonProperty(PropertyName = "last_update")]
        public string LastUpdated { get; set; }

        [JsonProperty(PropertyName = "my_tags")]
        public string MyTags { get; set; }
    }
}