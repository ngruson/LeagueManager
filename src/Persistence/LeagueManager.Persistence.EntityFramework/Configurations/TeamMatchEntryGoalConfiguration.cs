using LeagueManager.Domain.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamMatchEntryGoalConfiguration : IEntityTypeConfiguration<TeamMatchEntryGoal>
    {
        public void Configure(EntityTypeBuilder<TeamMatchEntryGoal> builder)
        {
            builder.Property(g => g.Guid).IsRequired();
            builder.Property(g => g.Minute).IsRequired();
            builder.HasOne(g => g.Player); //.IsRequired() doesn't work so this relation is optional for now
        }
    }
}