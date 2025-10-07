using LogiTrack.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly LogiTrackContext _context;

        public InventoryController(LogiTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetItems()
        {
            var items = _context.InventoryItems.ToList();
            return Ok(items);
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] InventoryItem item)
        {
            _context.InventoryItems.Add(item);
            _context.SaveChanges();
            return Ok(item);
        }
}
