using System.Collections.ObjectModel;
using System.Globalization;
using BubbleTesShop.Backend.Enums;

namespace BubbleTeaShop.Client.Converters;

public class ToppingToIsCheckedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ObservableCollection<Topping> selectedToppings && parameter is Topping topping)
        {
            return selectedToppings.Contains(topping);
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}