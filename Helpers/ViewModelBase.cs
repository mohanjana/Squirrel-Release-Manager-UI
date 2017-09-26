using System.ComponentModel;
using System.Threading.Tasks;

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

        public bool FormResult { get; set; }

        #endregion Properties

        #region MethodDefinitions

        public abstract void Save();
        public abstract void Close();
        public abstract bool ValidateForm();
        public abstract Task SaveAsync();
        public abstract Task CloseAsync();
        public abstract Task<bool> ValidateFormAsync();
        public abstract bool ValidatePropertyChanges(string propertyName);

        #endregion MethodDefinitions

        #region Methods

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            ValidatePropertyChanges(propertyName);
        }
        
        #endregion Methods
    }
}
