using LeagueManager.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(c => c.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(c => c.Code)
                .HasColumnType("varchar(5)")
                .IsRequired();
        }
    }
}