using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;
using Newtonsoft.Json;

namespace MAL.NetLogic.Classes
{
    public class MappingToJson : IMappingToJson
    {
        #region Variables

        private readonly IMappingEngine _mappingEngine;
        private readonly IAnimeFactory _factory;
        private readonly ICharacterFactory _characterFactory;

        #endregion

        #region Constructor

        public MappingToJson(IMappingEngine mappingEngine, IAnimeFactory factory, ICharacterFactory characterFactory)
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
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (ICharacterInformation), typeof (ICharacterInformationJson));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (ISeiyuuInformation), typeof (ISeiyuuInformationJson));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IAnimeography), typeof (IAnimeographyJson));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (IMangaography), typeof (IMangaographyJson));
            _mappingEngine.ConfigurationProvider.CreateTypeMap(typeof (ICharacterInformation), typeof (ICharacterInformationJson));
            _factory = factory;
            _characterFactory = characterFactory;
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
            string xmlString;
            var xmlVariant = _factory.CreateAnimeDetailsXml();
            _mappingEngine.Map(animeDetails, xmlVariant);
            var serializer = new XmlSerializer(typeof(AnimeDetailsXml));
            using (var mStream = new Utf8StringWriter())
            using (var streamWriter = XmlWriter.Create(mStream, new XmlWriterSettings {Encoding = Encoding.UTF8}))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(streamWriter, xmlVariant, ns);
                xmlString = mStream.ToString();
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

        public ICharacterInformation ConvertCharacterToJson(ICharacterInformationJson characterInfo)
        {
            var retChar = _characterFactory.CreateCharacter();
            _mappingEngine.Map(characterInfo, retChar);

            return retChar;
        }

        #endregion
    }
}