using LeagueManager.Domain.Round;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamLeagueRoundConfiguration : IEntityTypeConfiguration<TeamLeagueRound>
    {
        public void Configure(EntityTypeBuilder<TeamLeagueRound> builder)
        {
            builder.Property(tr => tr.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder. HasOne(tr => tr.TeamLeague)
                .WithMany(tl => tl.Rounds)
                .IsRequired();
        }
    }
}