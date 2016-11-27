using System;
using System.Xml.Serialization;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    [XmlRoot(ElementName = "entry")]
    public class AnimeDetailsXml : IAnimeDetailsXml
    {
        [XmlElement(ElementName = "episode")]
        public int Episodes { get; set; }

        [XmlElement(ElementName = "status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "score")]
        public int Score { get; set; }

        [XmlElement(ElementName = "storage_type")]
        public int StorageType { get; set; }

        [XmlElement(ElementName = "storage_value")]
        public float StorageValue { get; set; }

        [XmlElement(ElementName = "times_rewatched")]
        public int Rewatched { get; set; }

        [XmlElement(ElementName = "rewatch_value")]
        public int RewatchValue { get; set; }

        [XmlElement(ElementName = "date_start")]
        public string DateStart { get; set; }

        [XmlElement(ElementName = "date_finish")]
        public string DateFinish { get; set; }

        [XmlElement(ElementName = "priority")]
        public int Priority { get; set; }

        [XmlElement(ElementName = "enable_discussion")]
        public int EnableDiscussion { get; set; }

        [XmlElement(ElementName = "enable_rewatching")]
        public int EnableRewatching { get; set; }

        [XmlElement(ElementName = "comments")]
        public string Comments { get; set; }

        [XmlElement(ElementName = "tags")]
        public string Tags { get; set; }
    }
}