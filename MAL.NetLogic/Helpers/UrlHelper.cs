using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Helpers
{
    public class UrlHelper : IUrlHelper
    {
        public string MalUrl => @"http://myanimelist.net/anime/{0}";
        public string CleanMalUrl => @"http://myanimelist.net{0}";
        public string CharacterUrl => @"http://myanimelist.net/character/{0}";
        /// <summary>
        /// {0} is the year and {1} is the Season
        /// </summary>
        public string SeasonUrl => @"http://myanimelist.net/anime/season/{0}/{1}";
    }
}