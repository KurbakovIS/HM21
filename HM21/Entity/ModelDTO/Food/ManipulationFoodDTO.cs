using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HM21.Entity.ModelDTO
{
    public class ManipulationFoodDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public IFormFile Img { get; set; }
    }
}
