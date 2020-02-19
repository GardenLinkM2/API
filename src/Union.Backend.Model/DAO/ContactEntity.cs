using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    class ContactEntity : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(c => c.Id);
            builder
                .HasOne<User>()
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.Me);
            builder.Property(c => c.MyContact).IsRequired();
            builder.Property(c => c.FirstMessage).IsRequired();
            builder.Property(c => c.Status).IsRequired();
        }
    }
}
