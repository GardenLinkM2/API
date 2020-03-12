using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    class UserEntity : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.LastName).IsRequired();

            builder.HasOne(u => u.Photo);
            builder.HasOne(u => u.Wallet).WithOne().HasForeignKey<Wallet>(w => w.OfUser);
            
            builder.HasMany(u => u.Gardens).WithOne(g => g.Owner);
            builder.HasMany(u => u.AsRenter).WithOne(l => l.Renter);
        }
    }
}
