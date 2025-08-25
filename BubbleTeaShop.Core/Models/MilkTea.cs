using BubbleTeaShop.Core.Enums;

namespace BubbleTeaShop.Core.Models;

public class MilkTea : MenuItem
{
    public TeaBase TeaBase { get; set; }
    public MilkBase MilkBase { get; set; }
}