using HM21.Entity.ModelDTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HM21.Utility
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserAuth userForAuth);
        Task<string> CreateToken();
    }
}
