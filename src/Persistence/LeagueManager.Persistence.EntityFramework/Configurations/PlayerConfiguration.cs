using LeagueManager.Domain.Player;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(p => p.FirstName)
                .HasColumnType("varchar(50)");

            builder.Property(p => p.MiddleName)
                .HasColumnType("varchar(50)");

            builder.Property(p => p.LastName)
                .HasColumnType("varchar(50)");
        }
    }
}
