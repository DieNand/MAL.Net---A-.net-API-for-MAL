namespace MAL.NetLogic.Interfaces
{
    public interface IUrlHelper
    {
        string MalUrl { get; }
        string CharacterUrl { get; }
        string CleanMalUrl { get; }
        string SeasonUrl { get; }
    }
}