using Helpers.ViewModels;
using Helpers.WindowServices;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Helpers
{
    public class WindowService : IWindowService
    {
        public bool ShowWindow<T>() where T : Window, new()
        {
            T window = new T();
            return (bool)window.ShowDialog();
        }

        public bool ShowWindow<T>(object dataContext) where T : Window, new()
        {
            T window = new T();
            window.DataContext = dataContext;
            window.ShowDialog();
            return (bool)window.DialogResult;
        }

        public bool ShowWindow<T>(T form, object dataContext) where T : Window, new()
        {
            T window = form;
            window.DataContext = dataContext;
            return (bool)window.ShowDialog();
        }

        public void CloseWindow(ViewModelBase DataContext)
        {
            var window = Application.Current.Windows.Cast<Window>().Single(w => w.DataContext == this);
            window.DialogResult = DataContext.FormResult;
            window.Close();
        }
               
        public void CloseWindow(object DataContext, bool Result)
        {
            var window = Application.Current.Windows.Cast<Window>().Single(w => w.DataContext == this);
            window.DialogResult = Result;
            window.Close();
        }

        public async Task CloseWindowAsync(object DataContext)
        {
            var window = Application.Current.Windows.Cast<Window>().Single(w => w.DataContext == this);
            await Task.Run(() => window.Dispatcher.Invoke(() => { window.Close(); }));
        }

        public bool OpenFileDialog(string title, string filter, out string selectedFileName)
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                CheckFileExists = true,
                CheckPathExists = true,
                FilterIndex = 0,
                Multiselect = false,
                ValidateNames = true,
                Filter = filter
            };

            bool? result = dialog.ShowDialog();
            if (result ?? false)
            {
                selectedFileName = dialog.FileName;
                return true;
            }
            else
            {
                selectedFileName = null;
                return false;
            }
        }

        public bool OpenFolderDialog(string title, string initialPath, out string selectedPath)
        {
            var dialog = new VistaFolderBrowserDialog
            {
                ShowNewFolderButton = true,
                SelectedPath = initialPath,
                Description = title,
                UseDescriptionForTitle = true
            };

            bool? result = dialog.ShowDialog();
            if (result ?? false)
            {
                selectedPath = dialog.SelectedPath;
                return true;
            }
            else
            {
                selectedPath = null;
                return false;
            }
        }

        public MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None)
        {
            return MessageBox.Show(message, title, messageBoxButton, messageBoxImage);            
        }
    }
}
