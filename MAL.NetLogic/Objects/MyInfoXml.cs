using System;
using System.Xml.Serialization;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    [Serializable, XmlRoot("myinfo")]
    public class MyInfoXml : IMyInfoXml
    {
        [XmlElement(ElementName = "user_id")]
        public string UserId { get; set; }

        [XmlElement(ElementName = "user_name")]
        public string Username { get; set; }

        [XmlElement(ElementName = "user_watching")]
        public int Watching { get; set; }

        [XmlElement(ElementName = "user_completed")]
        public int Completed { get; set; }

        [XmlElement(ElementName = "user_onhold")]
        public int OnHold { get; set; }

        [XmlElement(ElementName = "user_dropped")]
        public int Dropped { get; set; }

        [XmlElement(ElementName = "user_plantowatch")]
        public int PlanToWatch { get; set; }

        [XmlElement(ElementName = "user_days_spent_watching")]
        public double DaysWatching { get; set; }
    }
}