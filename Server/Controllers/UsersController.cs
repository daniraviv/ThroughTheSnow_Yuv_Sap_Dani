using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThroughTheSnow_Yuv_Sap_Dani.Server.Data;
using ThroughTheSnow_Yuv_Sap_Dani.Shared.Entities;

namespace ThroughTheSnow_Yuv_Sap_Dani.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> UsersList = await _context.Users.ToListAsync();
            return Ok(UsersList);
        }



        [HttpGet("{mail}")]
        public async Task<IActionResult> LoginUser(string mail)
        {
            User userToReturn = await _context.Users.FirstOrDefaultAsync(u => u.Email == mail.ToLower());
			if (userToReturn != null)
			{
				HttpContext.Session.SetString("UserId", userToReturn.ID.ToString());
				return Ok(userToReturn.ID);
    }
			return BadRequest("User not found");



}

    }
}
