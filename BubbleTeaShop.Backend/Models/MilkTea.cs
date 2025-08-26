using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.Enums;

namespace BubbleTeaShop.Backend.Models;

public class MilkTea : MenuItem
{
    public TeaBase TeaBase { get; set; }
    public MilkBase MilkBase { get; set; }
}