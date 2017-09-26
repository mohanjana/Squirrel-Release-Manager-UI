using Helpers.WindowServices;
using System;
using System.Linq;
using System.Windows;

namespace Helpers
{
    public class WindowService : IWindowService
    {
        public bool ShowWindow<T>(object DataContext) where T : Window, new()
        {
            T window = new T();
            window.DataContext = DataContext;
            return (bool)window.ShowDialog();
        }

        public void CloseWindow(object DataContext)
        {
            var window = Application.Current.Windows.Cast<Window>().Single(w => w.DataContext == this);
            window.Close();
        }

        public void CloseWindow(object DataContext, bool Result)
        {
            var window = Application.Current.Windows.Cast<Window>().Single(w => w.DataContext == this);
            window.DialogResult = Result;
            window.Close();
        }
    }
}
