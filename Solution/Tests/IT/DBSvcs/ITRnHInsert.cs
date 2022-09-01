using DBSvcs.RnHsDBSvc;
using DBSvcs.SqlConnectionExtensions;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Common;
using Models.IDBSvc;
using Models.Results;
using System.Reflection.Metadata;

namespace Tests.IT.DBSvcs.RnHsDBSvcs
{
    [TestClass]
    public class ITRnHInsert : ITRnH
    {
        [TestMethod]
        public void TestRnHInsert()
        {
            RnH rnh = new RnH
            {
                Order = 2,
                HorseNo = 1,
                Status = RnHStatus.NotPresent,
                Mark = 0.0,
                IsRanked = false,
                IsDisqualificated = false
            };
            Result<RnH>? result = _dbSvc?.RnHInsert(rnh);
            CompareRnH(rnh, result, false);
        }
    }
}