using System;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class ListAnime : IListAnime
    {
        public int SeriesId { get; set; }
        public string SeriesTitle { get; set; }
        public string SeriesSynonyms { get; set; }
        public int SeriesType { get; set; }
        public int SeriesEpisodes { get; set; }
        public int SeriesStatus { get; set; }
        public string SeriesStart { get; set; }
        public string SeriesEnd { get; set; }
        public string SeriesImage { get; set; }
        public int MyId { get; set; }
        public int WatchedEpisodes { get; set; }
        public string MyStartDate { get; set; }
        public string MyFinishDate { get; set; }
        public int MyScore { get; set; }
        public int MyStatus { get; set; }
        public string MyRewatching { get; set; }
        public int RewatchingEpisode { get; set; }
        public string LastUpdated { get; set; }
        public string MyTags { get; set; }
    }
}