using LeagueManager.Domain.Sports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class TeamSportConfiguration : IEntityTypeConfiguration<TeamSports>
    {
        public void Configure(EntityTypeBuilder<TeamSports> builder)
        {
            builder.Property(ts => ts.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.HasOne(ts => ts.Options);
        }
    }
}