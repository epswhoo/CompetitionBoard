namespace CompetitionBoard_Net8.Models.Common
{
    public class RnH
    {
        public int Id { get; set; }
        public int HorseNo { get; set; }
        public int Order { get; set; }
        
        public RnHStatus Status { get; set; }

        public double Mark { get; set; }
        public bool IsDisqualificated { get; set; }
        public bool IsRanked { get; set; }
    }
}