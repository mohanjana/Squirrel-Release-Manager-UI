using MahApps.Metro.Controls;
using ReleaseManager.ViewModels;
using System.Windows;

namespace ReleaseManager.UI
{
    /// <summary>
    /// Interaction logic for FormMain.xaml
    /// </summary>
    public partial class FormMain : Window
    {
        public FormMain()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
