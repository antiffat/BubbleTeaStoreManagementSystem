using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Backend.DTOs.MenuItemDtos;

public class FruitTeaDto : MenuItemDto
{
    public FruitBase FruitBase { get; set; }
    public TeaBase TeaBase { get; set; }
    public int IceLevel { get; set; }
}

public class AddFruitTeaDto : AddMenuItemBaseDto
{
    public FruitBase FruitBase { get; set; }
    public TeaBase TeaBase { get; set; }
    public int IceLevel { get; set; }
}

public class UpdateFruitTeaDto : UpdateMenuItemBaseDto
{
    public FruitBase? FruitBase { get; set; }
    public TeaBase? TeaBase { get; set; }
    public int? IceLevel { get; set; }
}