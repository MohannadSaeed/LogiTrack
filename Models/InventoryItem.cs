using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogiTrack.Models;

public class InventoryItem
{
    [Key]
        public int ItemId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Location { get; set; }
        
        [ForeignKey("Order")]
        public int? OrderId { get; set; } 
        public void DisplayInfo()
    {
        Console.WriteLine($"Item: {Name} | Quantity: {Quantity} | Location: {Location}");
    }
}
