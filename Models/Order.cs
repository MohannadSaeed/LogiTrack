using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Models;

public class Order
{
    [Key]
        public int OrderId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public DateTime DatePlaced { get; set; } = DateTime.Now;

        // One-to-many relationship: one order can have multiple items
        public List<InventoryItem> Items { get; set; } = new List<InventoryItem>();

        public void AddItem(InventoryItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(int itemId)
        {
            var item = Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null)
                Items.Remove(item);
        }

        public string GetOrderSummary()
        {
            return $"Order #{OrderId} for {CustomerName} | Items: {Items.Count} | Placed: {DatePlaced.ToShortDateString()}";
        }
}
