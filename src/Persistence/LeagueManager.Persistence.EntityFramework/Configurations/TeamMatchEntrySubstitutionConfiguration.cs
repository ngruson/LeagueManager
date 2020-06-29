using LeagueManager.Domain.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamMatchEntrySubstitutionConfiguration : IEntityTypeConfiguration<TeamMatchEntrySubstitution>
    {
        public void Configure(EntityTypeBuilder<TeamMatchEntrySubstitution> builder)
        {
            builder.HasOne(s => s.TeamMatchEntry)
                .WithMany(me => me.Substitutions)
                .IsRequired();

            builder.Property(s => s.Minute)
                .HasColumnType("varchar(3)")
                .IsRequired();

            builder.HasOne(s => s.PlayerOut);

            builder.HasOne(s => s.PlayerIn);
        }
    }
}