using LeagueManager.Domain.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamLeagueMatchConfiguration : IEntityTypeConfiguration<TeamLeagueMatch>
    {
        public void Configure(EntityTypeBuilder<TeamLeagueMatch> builder)
        {
            //builder.Property(tm => tm.TeamLeagueRound)
            //    .IsRequired();
        }
    }
}