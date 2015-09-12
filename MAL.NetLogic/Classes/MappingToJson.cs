using System.IO;
using System.Xml.Serialization;
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
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IAnimeDetails), typeof (IAnimeDetailsJson));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IAnimeDetailsJson), typeof (IAnimeDetails));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IAnimeDetails), typeof (IAnimeDetailsXml));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IListAnimeXml), typeof (IListAnime));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IMyInfoXml), typeof (IMyInfo));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IMyAnimeList), typeof (IMyAnimeListJson));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IMyInfo), typeof (IMyInfoJson));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IListAnime), typeof (IListAnimeJson));
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

        public string ConvertAnimeDetailsToJson(IAnimeDetails animeDetails)
        {
            var jsonDetails = _factory.CreateJsonAnimeDetails();
            _mappingEngine.Map(animeDetails, jsonDetails);

            var jsonString = JsonConvert.SerializeObject(jsonDetails);
            return jsonString;
        }

        public string ConvertAnimeDetailsToXml(IAnimeDetails animeDetails)
        {
            var xmlString = string.Empty;
            var serializer = new XmlSerializer(typeof(IAnimeDetailsXml));
            using (var mStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(mStream))
            {
                serializer.Serialize(mStream, animeDetails);
                streamWriter.Write(xmlString);
            }
            return xmlString;
        }

        public string ConvertMyListToJson(IMyAnimeList animeList)
        {
            var retList = _factory.CreateJsonAnimeList();
            _mappingEngine.Map(animeList, retList);

            var jsonString = JsonConvert.SerializeObject(retList);
            return jsonString;
        }

        public IMyInfo ConvertMyInfoFromXmlToObject(IMyInfoXml info)
        {
            var myInfo = _factory.CreateMyInfo();
            _mappingEngine.Map(info, myInfo);

            return myInfo;
        }

        public IListAnime ConvertListAnimeXmlToObject(IListAnimeXml listAnime)
        {
            var retAnime = _factory.CreateListAnime();
            _mappingEngine.Map(listAnime, retAnime);

            return retAnime;
        }

        #endregion
    }
}