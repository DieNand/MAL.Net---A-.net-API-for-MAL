using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class Related : IRelated
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}