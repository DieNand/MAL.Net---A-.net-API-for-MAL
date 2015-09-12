namespace MAL.NetLogic.Interfaces
{
    public interface IAnimeFactory
    {
        IAnime CreateAnime();
        IAnimeOriginalJson CreateJsonAnime();
        IAnimeDetails CreateAnimeDetails();
        IAnimeDetailsJson CreateJsonAnimeDetails();
        IMyAnimeList CreateAnimeList();
        IMyAnimeListJson CreateJsonAnimeList();
        IListAnime CreateListAnime();
        IMyInfo CreateMyInfo();
    }
}