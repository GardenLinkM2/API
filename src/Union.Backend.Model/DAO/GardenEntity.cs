using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    class GardenEntity : IEntityTypeConfiguration<Garden>
    {
        public void Configure(EntityTypeBuilder<Garden> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.IdOwner).IsRequired();
            builder.Property(g => g.Name).IsRequired();
            builder.Property(g => g.Size).IsRequired();
            builder.Property(g => g.Type).IsRequired();
            builder.Property(g => g.MinUse).IsRequired();
            //builder.Property(g => g.Validation).IsRequired();
            //builder.Property(g => g.Criteria).IsRequired();
            //builder.Property(g => g.Reserve).IsRequired();

            builder.HasOne(g => g.Validation).WithOne().HasForeignKey<Validation>(v => v.ForGarden);
            builder.HasOne(g => g.Criteria).WithOne().HasForeignKey<Criteria>(c => c.ForGarden);
            builder.HasOne<User>().WithMany().HasForeignKey(g => g.IdOwner);

            builder.HasMany(g => g.Photos).WithOne().HasForeignKey(p => p.RelatedTo);

        }
    }
}
