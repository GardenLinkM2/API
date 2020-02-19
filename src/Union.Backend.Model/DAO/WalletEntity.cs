using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    class WalletEntity : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Balance).IsRequired();
        }
    }
}
