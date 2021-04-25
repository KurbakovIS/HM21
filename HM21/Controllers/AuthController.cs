using AutoMapper;
using HM21.Entity;
using HM21.Entity.Model;
using HM21.Entity.ModelDTO.Auth;
using HM21.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HM21.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DbContextFood _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authManager;

        public AuthController(IMapper mapper,
             UserManager<User> userManager,
             IAuthenticationManager authManager,
             DbContextFood context)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _authManager = authManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistration userRegistration)
        {
            var user = _mapper.Map<User>(userRegistration);
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                    ModelState.TryAddModelError(error.Code, error.Description);

                Console.WriteLine(ModelState);
                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Auth([FromBody] UserAuth user)
        {
            if (!await _authManager.ValidateUser(user))
                return Unauthorized("Authentication failed. Wrong user name or password");

            return Ok(new { Token = await _authManager.CreateToken() });
        }
    }
}
