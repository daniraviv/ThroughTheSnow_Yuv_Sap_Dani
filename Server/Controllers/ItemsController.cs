using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThroughTheSnow_Yuv_Sap_Dani.Server.Data;
using ThroughTheSnow_Yuv_Sap_Dani.Server.Helpers;
using ThroughTheSnow_Yuv_Sap_Dani.Shared.Entities;

namespace ThroughTheSnow_Yuv_Sap_Dani.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly FileStorage _fileStorage;

        public ItemsController(DataContext context, FileStorage fileStorage)
        {
           _context = context;
           _fileStorage = fileStorage;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            List<Item> ItemsList = await _context.Items.ToListAsync();
            return Ok(ItemsList);
        }

        [HttpGet("{gameid}")]
        public async Task<IActionResult> GetGameByCode(int gameid)
        {

            Game gameToReturn = await _context.Games.Include(g => g.GameItems).FirstOrDefaultAsync(g => g.ID == gameid);
            if (gameToReturn != null)
            {


                return Ok(gameToReturn);

            }
            return BadRequest("No such game");
        }


        [HttpGet("itemGame/{gameid}")]
        public async Task<IActionResult> GetitemBygame(int gameid)
        {

            Item gameToReturn = await _context.Items.FirstOrDefaultAsync(g => g.ID == gameid);
            if (gameToReturn != null)
            {


                return Ok(gameToReturn);

            }
            return BadRequest("No such game");
        }

        [HttpPost("InsertItem")]
        public async Task<IActionResult> AddItem(Item newItem)
        {
            if (newItem != null)
            {
                _context.Items.Add(newItem);
                await _context.SaveChangesAsync();

                return Ok(newItem);
            }
            else
            {
                return BadRequest("Item was not send");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateItem (Item ItemToUpdate)
        {
            Item ItemFromDB = await _context.Items.FirstOrDefaultAsync(i => i.ID == ItemToUpdate.ID);
            if (ItemFromDB != null)
            {
                ItemFromDB.ItemType = ItemToUpdate.ItemType;
                ItemFromDB.ItemContent = ItemToUpdate.ItemContent;
                ItemFromDB.IsCorrect = ItemToUpdate.IsCorrect;

                await _context.SaveChangesAsync();
                return Ok(ItemFromDB);

            }
            else
            {
                return BadRequest("no such item");
            }
        }

    [HttpDelete("{id}")]
        public async Task<IActionResult> DeletItem(int id)
        {
            Item ItemToDelete = await _context.Items.FirstOrDefaultAsync(i => i.ID == id);
            if (ItemToDelete != null)
            {
                _context.Items.Remove(ItemToDelete);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            else
            {
                return BadRequest("no such Item");
            }
        }


        [HttpGet("bycorr/{CorrBoll}")]
        public async Task<IActionResult> GetItemsByCorr(bool CorrBoll)
        {

            //    Item ItemCorr = new Item();
            //    ItemCorr = await _context.Items.Where(i => i.IsCorrect == CorrBoll).SingleAsync();

            //    return Ok(ItemCorr);
            //}
            List<Item> ItemsList = new List<Item>();
            ItemsList = await _context.Items.Where(w => w.IsCorrect == CorrBoll).ToListAsync();

            return Ok(ItemsList);
        }


        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadFile([FromBody] string imageBase64)
        {
            byte[] picture = Convert.FromBase64String(imageBase64);
            string url = await _fileStorage.SaveFile(picture, "png", "uploadedFiles");
            return Ok(url);
        }



        [HttpPost("deleteImages")]
        public async Task<IActionResult> DeleteImages([FromBody] List<string> images)
        {
            foreach (string img in images)
            {
                await _fileStorage.DeleteFile(img, "uploadedFiles");
            }
            return Ok("deleted");
        }





    }
}
