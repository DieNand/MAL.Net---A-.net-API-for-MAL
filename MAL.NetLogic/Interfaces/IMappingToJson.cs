namespace MAL.NetLogic.Interfaces
{
    public interface IMappingToJson
    {
        string ConvertAnimeToJson(IAnime anime);
        string ConvertAnimeDetailsToJson(IAnimeDetails animeDetails);
        string ConvertAnimeDetailsToXml(IAnimeDetails animeDetails);
        string ConvertMyListToJson(IMyAnimeList animeList);

        IMyInfo ConvertMyInfoFromXmlToObject(IMyInfoXml info);
        IListAnime ConvertListAnimeXmlToObject(IListAnimeXml listAnime);
        IAnimeDetails ConvertJsonAnimeDetailsToAnimeDetails(IAnimeDetailsJson jsonDetails);
    }
}