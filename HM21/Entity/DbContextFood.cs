using HM21.Entity.Configuration;
using HM21.Entity.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HM21.Entity
{
    public class DbContextFood:IdentityDbContext<User>
    {
         public DbContextFood(DbContextOptions options) : base(options) { }

        public DbSet<Food> Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new FoodConfiguration());
        }
    }
}
