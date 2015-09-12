using System;

namespace MAL.NetLogic.Interfaces
{
    public interface IAnimeDetailsXml
    {
        int Episodes { get; set; }
        string Status { get; set; }
        int Score { get; set; }
        string DownloadedEpisodes { get; set; }
        int StorageType { get; set; }
        float StorageValue { get; set; }
        int Rewatched { get; set; }
        int RewatchValue { get; set; }
        DateTime DateStart { get; set; }
        DateTime DateFinish { get; set; }
        int Priority { get; set; }
        int EnableDiscussion { get; set; }
        int EnableRewatching { get; set; }
        string Comments { get; set; }
        string FansubGroup { get; set; }
        string Tags { get; set; }
    }
}