using BubbleTeaShop.Core.Enums;

namespace BubbleTeaShop.Core.Models;

public class FruitTea : MenuItem
{
    public FruitBase FruitBase { get; set; }
    public TeaBase TeaBase { get; set; }
    public int IceLevel { get; set; }
}