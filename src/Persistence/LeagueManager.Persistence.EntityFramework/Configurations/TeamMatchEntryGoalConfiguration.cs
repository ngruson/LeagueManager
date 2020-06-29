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
            builder.Property(g => g.Minute).HasColumnType("varchar(3)").IsRequired();

            builder.HasOne(g => g.Player);

            builder.HasOne(g => g.TeamMatchEntry)
                .WithMany(me => me.Goals)
                .IsRequired();
        }
    }
}