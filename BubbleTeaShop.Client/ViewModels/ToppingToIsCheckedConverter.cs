using System.Collections.ObjectModel;
using System.Globalization;
using BubbleTesShop.Backend.Enums;

namespace BubbleTeaShop.Client.ViewModels;

public class ToppingToIsCheckedConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ObservableCollection<Topping> selectedToppings && parameter is Topping currentTopping)
        {
            return selectedToppings.Contains(currentTopping);
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("ConvertBack not needed for this scenario.");
    }
}