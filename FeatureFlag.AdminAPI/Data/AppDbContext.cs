using Microsoft.EntityFrameworkCore;
using FeatureFlag.AdminAPI.Models;

namespace FeatureFlag.AdminAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<FeatureFlag.AdminAPI.Models.FeatureFlag> FeatureFlags { get; set; }
    }
}
