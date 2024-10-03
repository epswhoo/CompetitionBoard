
using Models.Common;

namespace DBSvcs.RnHExtensions
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