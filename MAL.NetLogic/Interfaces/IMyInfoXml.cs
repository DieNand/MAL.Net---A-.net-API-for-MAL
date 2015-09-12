using System.Xml.Serialization;

namespace MAL.NetLogic.Interfaces
{
    public interface IMyInfoXml
    {
        [XmlElement(ElementName = "user_id")]
        string UserId { get; set; }

        [XmlElement(ElementName = "user_name")]
        string Username { get; set; }

        [XmlElement(ElementName = "user_watching")]
        int Watching { get; set; }

        [XmlElement(ElementName = "user_completed")]
        int Completed { get; set; }

        [XmlElement(ElementName = "user_onhold")]
        int OnHold { get; set; }

        [XmlElement(ElementName = "user_dropped")]
        int Dropped { get; set; }

        [XmlElement(ElementName = "user_plantowatch")]
        int PlanToWatch { get; set; }

        [XmlElement(ElementName = "user_days_spent_watching")]
        double DaysWatching { get; set; }
    }
}