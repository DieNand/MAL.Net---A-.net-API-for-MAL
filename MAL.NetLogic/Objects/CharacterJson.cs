using System.Collections.Generic;
using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Objects
{
    public class CharacterJson : ICharacterJson
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty(PropertyName = "biography")]
        public string Biography { get; set; }
        [JsonProperty(PropertyName = "favorited_count")]
        public int FavoriteCount { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "anime")]
        public List<IAnimeography> Anime { get; set; }
        [JsonProperty(PropertyName = "manga")]
        public List<IMangaography> Manga { get; set; }
        [JsonProperty(PropertyName = "error_message")]
        public string ErrorMessage { get; set; }
        [JsonProperty(PropertyName = "error_occured")]
        public bool ErrorOccured { get; set; }
    }
}