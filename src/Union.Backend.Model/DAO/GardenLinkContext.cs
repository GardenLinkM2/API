using Microsoft.EntityFrameworkCore;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public class GardenLinkContext : DbContext
    {
        public GardenLinkContext(DbContextOptions<GardenLinkContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            //TODO: vérifier les impacts et trouver comment faire la migration du Model
        }

        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<ContactAsk> ContactAsks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<GardenNotation> GardenNotations { get; set; }
        public DbSet<Garden> Gardens { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Photo<Message>> MessagePhotos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserNotation> UserNotations { get; set; }
        public DbSet<Photo<User>> UserPhotos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Using> Usings { get; set; }
        public DbSet<Validation> Validations { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.MapProduct();
            base.OnModelCreating(modelBuilder);
        }
    }
}
