using LeagueManager.Domain.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamMatchEntryConfiguration : IEntityTypeConfiguration<TeamMatchEntry>
    {
        public void Configure(EntityTypeBuilder<TeamMatchEntry> builder)
        {
            builder.HasOne(tme => tme.Team);

            builder.Property(tme => tme.HomeAway)
                .IsRequired();

            builder.HasOne(tme => tme.TeamLeagueMatch)
                .WithMany(m => m.MatchEntries)
                .IsRequired();

            builder.HasOne(tme => tme.Score);
        }
    }
}