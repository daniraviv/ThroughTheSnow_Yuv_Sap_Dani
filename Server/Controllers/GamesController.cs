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

        [HttpGet("byCode/{gameCode}")]
        public async Task<IActionResult> GetGameByCode(int gameCode)
        {
            Game gameToReturn = await _context.Games.FirstOrDefaultAsync(g => g.GameCode == gameCode);
            if (gameToReturn != null)
            {
                if (gameToReturn.GameName !=null)
                {
                    return Ok(gameToReturn.ID);
                }
                return BadRequest("הוסף שם משחק");
            }

            return BadRequest("No such game");
        }


        [HttpGet("playbyCode/{gameCode}")]
        public async Task<IActionResult> GetGameByCode2(int gameCode)
        {
            Game gameToReturn = await _context.Games.FirstOrDefaultAsync(g => g.GameCode == gameCode);
            if (gameToReturn != null)
            {
                if (gameToReturn.IsPublish == true)
                {
                    return Ok(gameToReturn.ID);
                }
                return BadRequest("game not published");
            }

            return BadRequest("No such game");
        }



        [HttpPost("Insert")]
        public async Task<IActionResult> AddGame(Game newGame)
        {
            string sessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionContent) == false) 
            {
                int SessionId = Convert.ToInt32(sessionContent);
                
                if (newGame != null)
                {
                    newGame.UserID = SessionId;
                    await _context.SaveChangesAsync();

                    
                    _context.Games.Add(newGame);
                    await _context.SaveChangesAsync();

                    newGame.GameCode = newGame.ID + 100;
                    await _context.SaveChangesAsync();


                    return Ok(newGame);
                }
                return BadRequest("game was not send");
      
                }
            return BadRequest("game was not send");

        }
           
        


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletGame(int id)
        {
            string sessionContent = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(sessionContent) == false)
            {
                int SessionId = Convert.ToInt32(sessionContent);
                Game GameToDelete = await _context.Games.FirstOrDefaultAsync(g => g.ID == id);
                if (GameToDelete != null)
                {
                    if (SessionId == GameToDelete.UserID)
                    {
                        _context.Games.Remove(GameToDelete);
                        await _context.SaveChangesAsync();
                        return Ok(true);
                    }

                    return BadRequest("Wrong user");
                }
                
                return BadRequest("No such game");
            }
            return BadRequest("empty session");
        }





        [HttpPost("Update")]

        public async Task<IActionResult> UpdateWorker(Game GameToUpdate)
        {
            Game GameFromDB = await _context.Games.FirstOrDefaultAsync(g => g.ID == GameToUpdate.ID);
            if (GameFromDB != null)
            {
                GameFromDB.GameName = GameToUpdate.GameName;
                GameFromDB.GameInstruction = GameToUpdate.GameInstruction;
                GameFromDB.IsPublish = GameToUpdate.IsPublish;

                await _context.SaveChangesAsync();
                return Ok(GameFromDB);

            }

            else
            {
                return BadRequest("no such Game");
            }


        }

    }
}
