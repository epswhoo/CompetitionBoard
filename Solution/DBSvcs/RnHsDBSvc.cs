//using Interfaces;
//using Models.IDBSvc;
//using Models.Common;
//using DBSvcs.SqlConnectionExtensions;

//namespace DBSvcs
//{
//    // All the code in this file is included in all platforms.
//    public class RnHsDBSvc : DBSvc, IRnHsDBSvc
//    {
//        public RnHsDBSvc(ConnectionStringDBSettingsExt sqlConnectionSvc) : base(sqlConnectionSvc)
//        {
            
//        }

//        DBResult<RnH> IRnHsDBSvc.DeleteRnH(RnH RnH)
//        {
//            throw new NotImplementedException();
//        }

//        DBResult<RnH> IRnHsDBSvc.InsertOnPosRnH(RnH RnH, int Pos)
//        {
//            throw new NotImplementedException();
//        }

//        DBResult<RnH> IRnHsDBSvc.InsertRnH(RnH RnH)
//        {
//            throw new NotImplementedException();
//        }

//        DBResult<RnH> IRnHsDBSvc.SaveRnH(RnH RnH)
//        {
//            throw new NotImplementedException();
//        }

//        //public DBResult<string> SaveTitle(string title)
//        //{
//        //    string cmd = $"UPDATE dbo.TitleTable SET Title = '{title}';";
//        //    return _sqlConnection.Execute(cmd);
//        //}
//    }
//}