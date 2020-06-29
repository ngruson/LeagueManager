using LeagueManager.Domain.Competitor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamCompetitorPlayerConfiguration : IEntityTypeConfiguration<TeamCompetitorPlayer>
    {
        public void Configure(EntityTypeBuilder<TeamCompetitorPlayer> builder)
        {
            builder.Property(tcp => tcp.Number)
                .HasColumnType("varchar(5)");

            builder.HasOne(x => x.Player);
        }
    }
}