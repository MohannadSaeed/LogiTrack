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

    // ✅ GET: /api/inventory
    [HttpGet]
    public IActionResult GetAllItems()
    {
        var items = _context.InventoryItems.ToList();
        return Ok(items);
    }

    // ✅ POST: /api/inventory
    [HttpPost]
    public IActionResult AddItem([FromBody] InventoryItem item)
    {
        if (item == null)
            return BadRequest("Item cannot be null.");

        _context.InventoryItems.Add(item);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllItems), new { id = item.ItemId }, item);
    }

    // ✅ DELETE: /api/inventory/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteItem(int id)
    {
        var item = _context.InventoryItems.Find(id);
        if (item == null)
            return NotFound($"Item with ID {id} not found.");

        _context.InventoryItems.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
