using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public class TalkEntity : IEntityTypeConfiguration<Talk>
    {
        public void Configure(EntityTypeBuilder<Talk> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasMany(t => t.Messages).WithOne().HasForeignKey(m => m.OfTalk);
        }
    }

    public class MessageEntity : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasOne<Talk>().WithMany().HasForeignKey(m => m.OfTalk);
        }
    }
}
