using BubbleTeaShop.Core.Enums;

namespace BubbleTeaShop.Core.Models;

public class Frappe : MenuItem
{
    public BaseFlavor BaseFlavor { get; set; }
    public bool HasWhippedCream { get; set; }
}