using LeagueManager.Domain.Competitor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamCompetitorConfiguration : IEntityTypeConfiguration<TeamCompetitor>
    {
        public void Configure(EntityTypeBuilder<TeamCompetitor> builder)
        {
            //builder.Property(tc => tc.Team)
            //    .IsRequired();
        }
    }
}