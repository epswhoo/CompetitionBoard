
using CompetitionBoard_Net8.Models.Common;

/* Nicht gemergte Änderung aus Projekt "DBSvcs (net6.0-maccatalyst)"
Vor:
using Microsoft.EntityFrameworkCore;
Nach:
using Microsoft.EntityFrameworkCore;
using DBSvcs;
using DBSvcs.RnHsDBSvc;
*/
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;

namespace CompetitionBoard_Net8.DBSvc.RnHsDBSvc
{
    // All the code in this file is included in all platforms.
    public class CompetitionBoardDBContext : DbContext
    {
        public CompetitionBoardDBContext(DbContextOptions<CompetitionBoardDBContext> options) : base(options)
        {
            //this.
            ////var test = 

            //((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 10; // seconds
        }

        //public MyDatabase()
        //: base(ContextHelper.CreateConnection("Connection string"), true)
        //{
        //    ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180; // seconds
        //}

        public DbSet<RnH> RnHsTable { get; set; }

        public DbSet<TitleDBContextModel> TitleTable { get; set; }
    }
}