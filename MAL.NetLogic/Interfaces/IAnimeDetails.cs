using System;

namespace MAL.NetLogic.Interfaces
{
    public interface IAnimeDetails
    {
        int AnimeId { get; set; }
        int Episodes { get; set; }
        string Status { get; set; }
        int Score { get; set; }
        string DownloadedEpisodes { get; set; }
        int StorageType { get; set; }
        float StorageValue { get; set; }
        int Rewatched { get; set; }
        int RewatchValue { get; set; }
        string DateStart { get; set; }
        string DateFinish { get; set; }
        int Priority { get; set; }
        int EnableDiscussion { get; set; }
        int EnableRewatching { get; set; }
        string Comments { get; set; }
        string FansubGroup { get; set; }
        string Tags { get; set; }
    }
}