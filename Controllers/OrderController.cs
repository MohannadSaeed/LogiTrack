using LogiTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly LogiTrackContext _context;

    public OrderController(LogiTrackContext context)
    {
        _context = context;
    }

    // ✅ GET: /api/orders
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        // ✅ Use AsNoTracking for read-only queries
    var orders = await _context.Orders
        .Include(o => o.Items) // eager load related data
        .AsNoTracking()
        .OrderByDescending(o => o.DatePlaced)
        .ToListAsync();

    return Ok(orders);
    }

    // ✅ GET: /api/orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _context.Orders
        .Include(o => o.Items)
        .AsNoTracking()
        .FirstOrDefaultAsync(o => o.OrderId == id);

    if (order == null)
        return NotFound();

    return Ok(order);
    }

    // ✅ POST: /api/orders
    [HttpPost]
    public IActionResult CreateOrder([FromBody] Order order)
    {
        if (order == null || order.Items == null || !order.Items.Any())
            return BadRequest("Order must include at least one item.");

        _context.Orders.Add(order);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
    }

    // ✅ DELETE: /api/orders/{id}
    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(int id)
    {
        var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.OrderId == id);
        if (order == null)
            return NotFound($"Order with ID {id} not found.");

        _context.Orders.Remove(order);
        _context.SaveChanges();
        return NoContent();
    }
}
