
using Microsoft.EntityFrameworkCore;
using Models.Common;

namespace TestProject1
{
    public class CompetitionBoardDataContext : DbContext
    {
        public CompetitionBoardDataContext(DbContextOptions<CompetitionBoardDataContext> options) : base(options)
        {
        }

        public DbSet<RnH> RnHs { get; set; }
    }
}
