using BubbleTesShop.Backend.Enums;

namespace BubbleTesShop.Backend.Models;

public class MilkTea : MenuItem
{
    public TeaBase TeaBase { get; set; }
    public MilkBase MilkBase { get; set; }
}