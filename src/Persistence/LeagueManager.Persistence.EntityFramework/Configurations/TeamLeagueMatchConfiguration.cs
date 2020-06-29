using LeagueManager.Domain.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamLeagueMatchConfiguration : IEntityTypeConfiguration<TeamLeagueMatch>
    {
        public void Configure(EntityTypeBuilder<TeamLeagueMatch> builder)
        {
            builder.HasOne(m => m.TeamLeagueRound)
                .WithMany(r => r.Matches)
                .IsRequired();
        }
    }
}