using System.Windows;

namespace Helpers.WindowServices
{
    interface IWindowService
    {
        bool ShowWindow<T>(object DataContext) where T : Window, new();
    }
}
