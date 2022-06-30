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
    public class GamesController : ControllerBase
    {
        private readonly DataContext _context;

        public GamesController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            List<Game> GamesList = await _context.Games.ToListAsync();
            return Ok(GamesList);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetallGame(int userId)
        {
            string SessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(SessionContent) == false)
            {
                if (userId == Convert.ToInt32(SessionContent))
                {
                    // User userToReturn = await _context.Users.Include(u => u.UserGames).ThenInclude(g=> g.GameMissions).FirstOrDefaultAsync(u => u.ID == userId); שליפה משתי טבלאות אחת אחרי השניה
                    User userToReturn = await _context.Users.Include(u => u.UserGames).FirstOrDefaultAsync(u => u.ID == userId);
                    if (userToReturn != null)
                    {
                        return Ok(userToReturn);
                    }
                    return BadRequest("User not found");
                }
                return BadRequest("User not login");
            }
            return BadRequest("Empty Session");
        }


    }
}
