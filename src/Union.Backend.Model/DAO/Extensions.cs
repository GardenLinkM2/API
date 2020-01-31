using Microsoft.EntityFrameworkCore;
using System;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public static class Extensions
    {
        public static ModelBuilder MapProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntity());
            modelBuilder.ApplyConfiguration(new PhotoUserEntity());
            modelBuilder.ApplyConfiguration(new PhotoMessageEntity());

            return modelBuilder;
        }
    }
}
