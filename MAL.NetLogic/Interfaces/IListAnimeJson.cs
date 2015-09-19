using System;

namespace MAL.NetLogic.Interfaces
{
    public interface IListAnimeJson
    {
        int SeriesId { get; set; }
        string SeriesTitle { get; set; }
        string SeriesSynonyms { get; set; }
        int SeriesType { get; set; }
        int SeriesEpisodes { get; set; }
        int SeriesStatus { get; set; }
        string SeriesStart { get; set; }
        string SeriesEnd { get; set; }
        string SeriesImage { get; set; }
        int MyId { get; set; }
        int WatchedEpisodes { get; set; }
        string MyStartDate { get; set; }
        string MyFinishDate { get; set; }
        int MyScore { get; set; }
        int MyStatus { get; set; }
        string MyRewatching { get; set; }
        int RewatchingEpisode { get; set; }
        string LastUpdated { get; set; }
        string MyTags { get; set; }
    }
}