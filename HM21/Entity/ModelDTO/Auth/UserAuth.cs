using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HM21.Entity.ModelDTO.Auth
{
    public class UserAuth
    {
        [Required(ErrorMessage = "User name is required"), MaxLength(20)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "User name is required"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
