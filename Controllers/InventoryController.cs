using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using LogiTrack.Models;

namespace LogiTrack.Controllers;
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly LogiTrackContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(LogiTrackContext context, IMemoryCache cache, ILogger<InventoryController> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        // ✅ GET: api/inventory (with caching)
        [HttpGet]
        public async Task<IActionResult> GetInventoryItems()
        {
            const string cacheKey = "inventory_list";

            if (!_cache.TryGetValue(cacheKey, out List<InventoryItem>? items))
            {
                _logger.LogInformation("Cache miss — fetching inventory from DB...");

                items = await _context.InventoryItems
                    .AsNoTracking()
                    .OrderBy(i => i.Name)
                    .ToListAsync();

                // Store in cache for 30 seconds
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(10)
                };

                _cache.Set(cacheKey, items, cacheOptions);
            }
            else
            {
                _logger.LogInformation("Cache hit — returning cached inventory list.");
            }

            return Ok(items);
        }

        // ✅ POST: api/inventory
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] InventoryItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

            // Clear cache when data changes
            _cache.Remove("inventory_list");

            return CreatedAtAction(nameof(GetInventoryItems), new { id = item.ItemId }, item);
        }

        // ✅ DELETE: api/inventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();

            // Invalidate cache
            _cache.Remove("inventory_list");

            return NoContent();
        }
    }
