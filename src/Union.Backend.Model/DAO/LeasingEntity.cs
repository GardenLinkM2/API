using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public class LeasingEntity : IEntityTypeConfiguration<Leasing>
    {
        public void Configure(EntityTypeBuilder<Leasing> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Begin).IsRequired();
            builder.Property(l => l.End).IsRequired();
            builder.Property(l => l.Garden).IsRequired();
            builder.Property(l => l.Owner).IsRequired();
            builder.Property(l => l.Price).IsRequired();
            builder.Property(l => l.Renew).IsRequired();
            builder.Property(l => l.Renter).IsRequired();
            builder.Property(l => l.State).IsRequired();
            builder.Property(l => l.Time).IsRequired();

            //builder.HasOne(l => l.Owner).WithOne().HasForeignKey<User>(u => u.Id);
            builder.HasOne(l => l.Renter).WithOne().HasForeignKey<User>(u => u.Id);
            builder.HasOne(l => l.Garden).WithOne().HasForeignKey<Garden>(g => g.Id);
        }
    }
}
