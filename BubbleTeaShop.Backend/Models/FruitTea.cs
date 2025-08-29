using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;

namespace BubbleTeaShop.Backend.Models;

public class FruitTea : MenuItem
{
    public FruitBase FruitBase { get; set; }
    public TeaBase TeaBase { get; set; }
    public int IceLevel { get; set; }
}