namespace BubbleTeaShop.Backend.DTOs.MenuItemDtos;

public class MenuItemDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double BasePrice { get; set; }
    public int StockQuantity { get; set; }
    public List<string> Allergens { get; set; } = new();
    public string ItemType { get; set; }
}

public abstract class AddMenuItemBaseDto
{
    public string Name { get; set; }
    public double BasePrice { get; set; }
    public int StockQuantity { get; set; } = 0;
    public IEnumerable<string> Allergens { get; set; }
}

public abstract class UpdateMenuItemBaseDto
{
    public int Id { get; set; } 
    public string? Name { get; set; }
    public double? BasePrice { get; set; }
    public int? StockQuantity { get; set; }
    public IEnumerable<string> Allergens { get; set; } 
}