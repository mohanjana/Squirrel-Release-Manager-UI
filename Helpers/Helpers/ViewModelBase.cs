using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Helpers.WindowServices;
using System.Linq;

namespace Helpers.ViewModels
{
    public abstract class ViewModelBase : WindowService, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Variables

        private bool WindowResult { get; set; }

        #endregion Variables

        #region Properties

        public object FormResult { get; set; }

        #endregion Properties

        #region MethodDefinitions

        public abstract void Save();
        public abstract void Close();
        public abstract bool ValidateForm();
        public abstract Task SaveAsync();
        public abstract Task CloseAsync();
        public abstract Task<bool> ValidateFormAsync();

        #endregion MethodDefinitions

        #region Methods

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}
