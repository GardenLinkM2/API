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
    }
}
