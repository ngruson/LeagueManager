using LeagueManager.Domain.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamMatchEntryLineupEntryConfiguration : IEntityTypeConfiguration<TeamMatchEntryLineupEntry>
    {
        public void Configure(EntityTypeBuilder<TeamMatchEntryLineupEntry> builder)
        {
            builder.HasOne(le => le.TeamMatchEntry)
                .WithMany(me => me.Lineup)
                .IsRequired();

            builder.Property(le => le.Number)
                .HasColumnType("varchar(3)")
                .IsRequired();

            builder.HasOne(le => le.Player);
        }
    }
}