using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    class GardenEntity : IEntityTypeConfiguration<Garden>
    {
        public void Configure(EntityTypeBuilder<Garden> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name).IsRequired();
            builder.Property(g => g.MinUse).IsRequired();
            builder.Property(g => g.Validation).IsRequired();
            
            builder.HasOne(g => g.Owner).WithMany();
            builder.HasMany(g => g.Photos).WithOne();
            builder.HasMany(g => g.Leasings).WithOne(l => l.Garden);
        }
    }
}
