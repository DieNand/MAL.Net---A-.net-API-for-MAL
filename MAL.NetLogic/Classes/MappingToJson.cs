using AutoMapper;
using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Classes
{
    public class MappingToJson : IMappingToJson
    {
        #region Variables

        private readonly IMappingEngine _mappingEngine;
        private readonly IAnimeFactory _factory;

        #endregion

        #region Constructor

        public MappingToJson(IMappingEngine mappingEngine, IAnimeFactory factory)
        {
            _mappingEngine = mappingEngine;
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IAnime), typeof (IAnimeOriginalJson));
            _factory = factory;
        }

        #endregion

        #region Public Methods

        public string ConvertAnimeToJson(IAnime anime)
        {
            var jsonAnime = _factory.CreateJsonAnime();
            _mappingEngine.Map(anime, jsonAnime);
            jsonAnime.OtherTitles["japanese"].AddRange(anime.JapaneseTitles);
            jsonAnime.OtherTitles["english"].AddRange(anime.EnglishTitles);
            jsonAnime.OtherTitles["synonyms"].AddRange(anime.SynonymousTitles);

            var jsonString = JsonConvert.SerializeObject(jsonAnime);
            return jsonString;
        }

        #endregion
    }
}