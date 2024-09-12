﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExemploAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersControllers : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersControllers(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByNameAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users != null)
            {
                return Ok(users);
            }
            return NoContent();
        }

    }
}
