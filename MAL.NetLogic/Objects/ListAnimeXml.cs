using System;
using System.Xml.Serialization;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    [Serializable, XmlRoot("anime")]
    public class ListAnimeXml : IListAnimeXml
    {
        [XmlElement(ElementName = "series_animedb_id")]
        public int SeriesId { get; set; }

        [XmlElement(ElementName = "series_title")]
        public string SeriesTitle { get; set; }

        [XmlElement(ElementName = "series_synonyms")]
        public string SeriesSynonyms { get; set; }

        [XmlElement(ElementName = "series_type")]
        public int SeriesType { get; set; }

        [XmlElement(ElementName = "series_episodes")]
        public int SeriesEpisodes { get; set; }

        [XmlElement(ElementName = "series_status")]
        public int SeriesStatus { get; set; }

        [XmlElement(ElementName = "series_start")]
        public string SeriesStart { get; set; }

        [XmlElement(ElementName = "series_end")]
        public string SeriesEnd { get; set; }

        [XmlElement(ElementName = "series_image")]
        public string SeriesImage { get; set; }

        [XmlElement(ElementName = "my_id")]
        public int MyId { get; set; }

        [XmlElement(ElementName = "my_watched_episodes")]
        public int WatchedEpisodes { get; set; }

        [XmlElement(ElementName = "my_start_date")]
        public string MyStartDate { get; set; }

        [XmlElement(ElementName = "my_finish_date")]
        public string MyFinishDate { get; set; }

        [XmlElement(ElementName = "my_score")]
        public int MyScore { get; set; }

        [XmlElement(ElementName = "my_status")]
        public int MyStatus { get; set; }

        [XmlElement(ElementName = "my_rewatching")]
        public string MyRewatching { get; set; }

        [XmlElement(ElementName = "my_rewatching_ep")]
        public int RewatchingEpisode { get; set; }

        [XmlElement(ElementName = "my_last_updated")]
        public string LastUpdated { get; set; }

        [XmlElement(ElementName = "my_tags")]
        public string MyTags { get; set; }
    }
}