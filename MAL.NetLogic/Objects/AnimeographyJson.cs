using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Objects
{
    public class AnimeographyJson : IAnimeographyJson
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}