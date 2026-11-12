using GxDicto.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GxDicto.Api.Data
{
    public class LocalizationDbContext : DbContext
    {
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options)
            : base(options) { }

        public DbSet<Gdbse> Gdbases { get; set; }
    }
}
