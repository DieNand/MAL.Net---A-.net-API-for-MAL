using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Objects
{
    public class SeiyuuInformationJson : ISeiyuuInformationJson
    {
        [JsonProperty(PropertyName = "image_url")]
        public string PictureUrl { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
    }
}