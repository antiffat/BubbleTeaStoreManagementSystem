using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;

namespace BubbleTeaShop.Backend.Models;

public class Frappe : MenuItem
{
    public BaseFlavor BaseFlavor { get; set; }
    public bool HasWhippedCream { get; set; }
}