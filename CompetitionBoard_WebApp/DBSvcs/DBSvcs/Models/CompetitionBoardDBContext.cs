using Base.Models.RnHs;
using Microsoft.EntityFrameworkCore;

namespace DBSvcs.Models
{
    public class CompetitionBoardDBContext : DbContext
    {
        public CompetitionBoardDBContext(DbContextOptions<CompetitionBoardDBContext> options) : base(options)
        {

        }

        public DbSet<RnH> RnHsTable { get; set; }

        public DbSet<TitleDBContextModel> TitleTable { get; set; }
    }
}
