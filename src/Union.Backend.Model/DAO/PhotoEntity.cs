using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    class PhotoUserEntity : IEntityTypeConfiguration<Photo<User>>
    {
        public void Configure(EntityTypeBuilder<Photo<User>> builder)
        {
            builder.HasKey(p => p.Id);
            //builder
            //    .Property(p => p.)
            //    .WithOne(u => u.Photo)
            //    .HasForeignKey(p => p.RelatedTo);
        }
    }

    class PhotoMessageEntity : IEntityTypeConfiguration<Photo<Message>>
    {
        public void Configure(EntityTypeBuilder<Photo<Message>> builder)
        {
            //builder.HasKey(p => p.Id);
            //builder.HasOne(p => p.RelatedTo).WithMany(u => u.Photos).HasForeignKey(p => p.RelatedToForeingKey);
        }
    }
}
