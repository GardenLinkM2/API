using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public class ScoreEntity : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Mark).IsRequired();
            builder.Property(s => s.Comment).IsRequired();
            builder.Property(s => s.Reported).IsRequired();
            builder.Property(s => s.Rated).IsRequired();
            
            builder.HasOne(s => s.Rater).WithMany(u => u.AsRater);
        }
    }
}
