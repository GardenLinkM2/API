using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public static class Extensions
    {
        public async static Task<T> GetByIdAsync<T>(this IQueryable<T> queryable, Guid id)
            where T : UniqueEntity
        {
            try
            {
                return await queryable.FirstOrDefaultAsync(e => e.Id.Equals(id));
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<T> GetByIdAsync<T>(this DbSet<T> set, Guid id)
            where T : UniqueEntity
        {
            try
            {
                return await set.FirstOrDefaultAsync(e => e.Id.Equals(id));
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static ModelBuilder MapProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactEntity());
            modelBuilder.ApplyConfiguration(new GardenEntity());
            modelBuilder.ApplyConfiguration(new LeasingEntity());
            modelBuilder.ApplyConfiguration(new PaymentEntity());
            modelBuilder.ApplyConfiguration(new PhotoUserEntity());
            modelBuilder.ApplyConfiguration(new PhotoMessageEntity());
            modelBuilder.ApplyConfiguration(new ScoreEntity());
            modelBuilder.ApplyConfiguration(new TalkEntity());
            modelBuilder.ApplyConfiguration(new UserEntity());
            modelBuilder.ApplyConfiguration(new WalletEntity());

            return modelBuilder;
        }
    }
}
