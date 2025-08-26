using BubbleTesShop.Backend.Enums;

namespace BubbleTesShop.Backend.Models;

public class Frappe : MenuItem
{
    public BaseFlavor BaseFlavor { get; set; }
    public bool HasWhippedCream { get; set; }
}