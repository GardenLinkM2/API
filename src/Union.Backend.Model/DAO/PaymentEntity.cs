using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public class PaymentEntity : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Sum).IsRequired();
        }
    }
}
