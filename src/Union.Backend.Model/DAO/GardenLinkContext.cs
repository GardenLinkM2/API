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

        public DbSet<Criteria> Characteristics { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Talk> Conversations { get; set; }
        public DbSet<GardenNotation> GardenNotations { get; set; }
        public DbSet<Garden> Gardens { get; set; }
        public DbSet<Leasing> Locations { get; set; }
        public DbSet<Photo<Message>> MessagePhotos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Talk> Talks { get; set; }
        public DbSet<UserNotation> UserNotations { get; set; }
        public DbSet<Photo<User>> UserPhotos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Validation> Validations { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.MapProduct();
            base.OnModelCreating(modelBuilder);
        }
    }
}
