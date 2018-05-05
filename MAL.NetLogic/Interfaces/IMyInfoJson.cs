namespace MAL.NetLogic.Interfaces
{
    public interface IMyInfoJson
    {
        string UserId { get; set; }
        string Username { get; set; }
        int Watching { get; set; }
        int Completed { get; set; }
        int OnHold { get; set; }
        int Dropped { get; set; }
        int PlanToWatch { get; set; }
        double DaysWatching { get; set; }
    }
}