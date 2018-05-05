using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class MyInfo : IMyInfo
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int Watching { get; set; }
        public int Completed { get; set; }
        public int OnHold { get; set; }
        public int Dropped { get; set; }
        public int PlanToWatch { get; set; }
        public double DaysWatching { get; set; } 
    }
}