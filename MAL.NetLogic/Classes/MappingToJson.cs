using AutoMapper;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;
using Newtonsoft.Json;

namespace MAL.NetLogic.Classes
{
    public class MappingToJson
    {
        #region Constructor

        public MappingToJson()
        {
            Mapper.CreateMap<Anime, AnimeOriginalJson>();
        }

        #endregion

        #region Public Methods

        public string ConvertAnimeToJson(IAnime anime, IAnimeOriginalJson jsonAnime)
        {
            Mapper.Map(anime, jsonAnime);
            jsonAnime.OtherTitles["japanese"].AddRange(anime.JapaneseTitles);
            jsonAnime.OtherTitles["english"].AddRange(anime.EnglishTitles);
            jsonAnime.OtherTitles["synonyms"].AddRange(anime.SynonymousTitles);

            var jsonString = JsonConvert.SerializeObject(jsonAnime);
            return jsonString;
        }

        #endregion
    }
}