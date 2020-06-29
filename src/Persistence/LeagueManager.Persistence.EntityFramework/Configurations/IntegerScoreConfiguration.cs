using LeagueManager.Domain.Score;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueManager.Persistence.EntityFramework.Configurations
{
    public class IntegerScoreConfiguration : IEntityTypeConfiguration<IntegerScore>
    {
        public void Configure(EntityTypeBuilder<IntegerScore> builder)
        {
            builder.Property(s => s.Value)
                .IsRequired();
        }
    }
}