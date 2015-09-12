using System;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class AnimeDetails : IAnimeDetails
    {
        public int AnimeId { get; set; }
        public int Episodes { get; set; }
        public string Status { get; set; }
        public int Score { get; set; }
        public string DownloadedEpisodes { get; set; }
        public int StorageType { get; set; }
        public float StorageValue { get; set; }
        public int Rewatched { get; set; }
        public int RewatchValue { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public int Priority { get; set; }
        public int EnableDiscussion { get; set; }
        public int EnableRewatching { get; set; }
        public string Comments { get; set; }
        public string FansubGroup { get; set; }
        public string Tags { get; set; }
    }
}