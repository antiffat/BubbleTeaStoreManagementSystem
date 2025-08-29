using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;

namespace BubbleTeaShop.Backend.Models;

public class MilkTea : MenuItem
{
    public TeaBase TeaBase { get; set; }
    public MilkBase MilkBase { get; set; }
}