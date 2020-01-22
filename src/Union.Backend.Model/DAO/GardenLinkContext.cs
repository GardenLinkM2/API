using Microsoft.EntityFrameworkCore;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public class GardenLinkContext : DbContext
    {
        public GardenLinkContext(DbContextOptions<GardenLinkContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactAsk> ContactAsks { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Garden> Gardens { get; set; }
        public DbSet<GardenNotation> GardenNotations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserNotation> UserNotations { get; set; }
        public DbSet<Using> Usings { get; set; }
        public DbSet<Validation> Validations { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

    }
}
