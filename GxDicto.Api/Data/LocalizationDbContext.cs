using GxDicto.DModels;

using Microsoft.EntityFrameworkCore;

namespace GxDicto.Api.Data
{
    public class LocalizationDbContext : DbContext
    {
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options)
            : base(options) { }

        public DbSet<LocalizationEntry> LocalizationEntries { get; set; }
    }
}
