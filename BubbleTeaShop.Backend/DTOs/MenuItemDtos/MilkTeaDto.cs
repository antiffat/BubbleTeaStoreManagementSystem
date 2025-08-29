using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Backend.DTOs.MenuItemDtos;

public class MilkTeaDto : MenuItemDto
{
    public TeaBase TeaBase { get; set; }
    public MilkBase MilkBase { get; set; }
}

public class AddMilkTeaDto : AddMenuItemBaseDto
{
    public TeaBase TeaBase { get; set; }
    public MilkBase MilkBase { get; set; }
}

public class UpdateMilkTeaDto : UpdateMenuItemBaseDto
{
    public TeaBase? TeaBase { get; set; }
    public MilkBase? MilkBase { get; set; }
}