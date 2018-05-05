using System.Collections.Generic;
using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Objects
{
    public class CharacterInformationJson : ICharacterInformationJson
    {
        [JsonProperty(PropertyName = "image_url")]
        public string CharacterPicture { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string CharacterName { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string CharacterUrl { get; set; }
        [JsonProperty(PropertyName = "character_type")]
        public string CharacterType { get; set; }
        [JsonProperty(PropertyName = "seiyuu")]
        public List<ISeiyuuInformationJson> Seiyuu { get; set; }
    }
}