using Fodraszat.Data.Entities;
using Fodraszat.Data.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fodraszat.Data
{
    public class AppDbContext : IdentityDbContext<Felhasznalo, IdentityRole<int>, int> 
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        { }

        public DbSet<Idopont> Idopontok { get; set; }
        public DbSet<Nyitvatartas> Nyitvatartas { get; set; }
        public DbSet<Szolgaltatas> Szolgaltatasok { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.AddInterceptors(SimpleAuditInterceptor.Instance);
    }
}
