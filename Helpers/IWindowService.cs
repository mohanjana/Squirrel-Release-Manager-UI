using System.Windows;

namespace Helpers.WindowServices
{
    public interface IWindowService
    {
        bool ShowWindow<T>() where T : Window, new();
        bool ShowWindow<T>(object dataContext) where T : Window, new();
        bool ShowWindow<T>(T form, object dataContext) where T : Window, new();
        bool OpenFileDialog(string title, string filter, out string selectedFileName);
        bool OpenFolderDialog(string title, string initialPath, out string selectedPath);
        MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None);
    }
}
