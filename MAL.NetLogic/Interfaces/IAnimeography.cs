namespace MAL.NetLogic.Interfaces
{
    public interface IAnimeography
    {
        int Id { get; set; }
        string Name { get; set; }
        string ImageUrl { get; set; }
        string Url { get; set; }
    }
}