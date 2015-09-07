namespace MAL.NetLogic.Interfaces
{
    public interface IAnimeFactory
    {
        IAnime CreateAnime();
        IAnimeOriginalJson CreateJsonAnime();
    }
}