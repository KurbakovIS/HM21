using HM21.Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HM21.Entity.Configuration
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.HasData
             (
                 new Food
                 {
                     Id = 1,
                     Name = "Mexican Eggrolls",
                     Description = "Face together given moveth divided form Of Seasons that fruitful",
                     Price = 14.50m
                 },
                 new Food
                 {
                     Id =2,
                     Name = "Chicken Burger",
                     Description = "Face together given moveth divided form Of Seasons that fruitful",
                     Price = 9.50m
                 },
                  new Food
                  {
                      Id = 3,
                      Name = "Topu Lasange",
                      Description = "Face together given moveth divided form Of Seasons that fruitful",
                      Price = 12.50m
                  }
             );
        }
    }
}
