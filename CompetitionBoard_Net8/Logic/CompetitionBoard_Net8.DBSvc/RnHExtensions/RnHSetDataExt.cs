
using CompetitionBoard_Net8.Models.Common;

namespace CompetitionBoard_Net8.DBSvc.RnHExtensions
{
    public static class RnHSetDataExt
    {
        public static void SetData(this RnH rnh, RnH source)
        {
            rnh.Id = source.Id;
            rnh.Order = source.Order;
            rnh.HorseNo = source.HorseNo;
            rnh.Mark = source.Mark;
            rnh.Status = source.Status;
            rnh.IsDisqualificated = source.IsDisqualificated;
            rnh.IsRanked = source.IsRanked;
        }
    }
}