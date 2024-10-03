
using Models.Common;

/* Nicht gemergte Änderung aus Projekt "DBSvcs (net6.0-maccatalyst)"
Vor:
using Microsoft.EntityFrameworkCore;
Nach:
using Microsoft.EntityFrameworkCore;
using DBSvcs;
using DBSvcs.RnHsDBSvc;
*/
using Microsoft.EntityFrameworkCore;

namespace DBSvcs.RnHsDBSvc
{
    // All the code in this file is included in all platforms.
    public class CompetitionBoardDBContext : DbContext
    {
        public CompetitionBoardDBContext(DbContextOptions<CompetitionBoardDBContext> options) : base(options)
        {

        }

        public DbSet<RnH> RnHsTable { get; set; }

        public DbSet<TitleDBContextModel> TitleTable { get; set; }
    }
}