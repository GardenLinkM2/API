using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public class PaymentEntity : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Sum).IsRequired();
            builder.Property(p => p.State).IsRequired();
            builder.Property(p => p.Leasing).IsRequired();


            builder.HasOne(p => p.Leasing).WithOne().HasForeignKey<Leasing>(l => l.Id);

        }
    }
}
