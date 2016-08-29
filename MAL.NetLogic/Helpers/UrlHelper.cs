using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Helpers
{
    public class UrlHelper : IUrlHelper
    {
        public string MalUrl => @"https://myanimelist.net/anime/{0}";
        public string CleanMalUrl => @"https://myanimelist.net{0}";
        public string CharacterUrl => @"https://myanimelist.net/character/{0}";
        /// <summary>
        /// {0} is the year and {1} is the Season
        /// </summary>
        public string SeasonUrl => @"https://myanimelist.net/anime/season/{0}/{1}";
    }
}