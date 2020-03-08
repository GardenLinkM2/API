using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
            builder.Property(l => l.Renew).IsRequired();
            builder.Property(l => l.State).IsRequired();
            builder.Property(l => l.Time).IsRequired();

            builder.HasOne(l => l.Payment).WithOne(p => p.Leasing).HasForeignKey<Payment>(p => p.OfLeasing);
        }
    }
}
