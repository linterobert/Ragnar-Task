using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RagnarApp.Domain.Entities;

namespace RagnarApp.Infrastructure.Configurations
{
    internal class LibraryConfiguration : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            builder.ToTable("library");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(x => x.Author)
                .HasColumnName("author")
                .IsRequired();

            builder.Property(x => x.Genre)
                .HasColumnName("genre")
                .IsRequired();

            builder.Property(x => x.ImportDate)
                .HasColumnName("import_date")
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        }
    }
}
