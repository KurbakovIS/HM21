using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuApp.Entity.ModelsDto
{
    class FoodForUpdateDto: FoodForManipulationDto
    {
        public int Id { get; set; }
    }
}
