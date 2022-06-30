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
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemsController(DataContext context)
        {
           _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            List<Item> ItemsList = await _context.Items.ToListAsync();
            return Ok(ItemsList);
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

        [HttpPost("Update/{id}")]
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






    }
}
