using Microsoft.EntityFrameworkCore;
using RagnarApp.Domain.Entities;
using RagnarApp.Infrastructure.Configurations;

namespace RagnarApp.Infrastructure.Data
{
    public class RagnarAppContext : DbContext
    {
        public RagnarAppContext() { }
        public RagnarAppContext(DbContextOptions<RagnarAppContext> options) : base(options) { }
        public DbSet<Library> Libraries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new LibraryConfiguration());
        }
    }
}
