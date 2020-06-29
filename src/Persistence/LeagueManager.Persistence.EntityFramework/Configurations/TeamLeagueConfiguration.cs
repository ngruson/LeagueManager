using LeagueManager.Domain.Competition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamLeagueConfiguration : IEntityTypeConfiguration<TeamLeague>
    {
        public void Configure(EntityTypeBuilder<TeamLeague> builder)
        {
            builder.Property(tl => tl.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.HasMany(tl => tl.Competitors);
        }
    }
}