using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Backend.DTOs.MenuItemDtos;

public class FrappeDto : MenuItemDto
{
    public BaseFlavor BaseFlavor { get; set; }
    public bool HasWhippedCream { get; set; }
}

public class AddFrappeDto : AddMenuItemBaseDto
{
    public BaseFlavor BaseFlavor { get; set; }
    public bool HasWhippedCream { get; set; }
}

public class UpdateFrappeDto : UpdateMenuItemBaseDto
{
    public BaseFlavor? BaseFlavor { get; set; }
    public bool? HasWhippedCream { get; set; }
}