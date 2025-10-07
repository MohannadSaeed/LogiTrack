using LogiTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Controllers;

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
    public IActionResult GetAllOrders()
    {
        var orders = _context.Orders
            .Include(o => o.Items)
            .ToList();

        return Ok(orders);
    }

    // ✅ GET: /api/orders/{id}
    [HttpGet("{id}")]
    public IActionResult GetOrderById(int id)
    {
        var order = _context.Orders
            .Include(o => o.Items)
            .FirstOrDefault(o => o.OrderId == id);

        if (order == null)
            return NotFound($"Order with ID {id} not found.");

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
