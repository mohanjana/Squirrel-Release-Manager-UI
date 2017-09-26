using Helpers.Commands;
using Helpers.Files;
using Helpers.Json;
using Helpers.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ReleaseManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Variables

        private AppSetting appSetting;

        #endregion

        #region Properties

        private string nuGetSpecFile;

        public string NuGetSpecFile
        {
            get { return nuGetSpecFile; }
            set
            {
                nuGetSpecFile = value;
                OnPropertyChanged("NuGetSpecFile");
            }
        }

        private string packageFileLocaton;

        public string PackageFileLocaton
        {
            get { return packageFileLocaton; }
            set
            {
                packageFileLocaton = value;
                OnPropertyChanged("PackageFileLocaton");
            }
        }

        private string nuGetExecutable;

        public string NuGetExecutable
        {
            get { return nuGetExecutable; }
            set
            {
                nuGetExecutable = value;
                OnPropertyChanged("NuGetExecutable");
            }
        }


        private string squirrelExecutable;

        public string SquirrelExecutable
        {
            get { return squirrelExecutable; }
            set
            {
                squirrelExecutable = value;
                OnPropertyChanged("SquirrelExecutable");
            }

        }

        private string releasePath;

        public string ReleasePath
        {
            get { return releasePath; }
            set
            {
                releasePath = value;
                OnPropertyChanged("ReleasePath");
            }
        }

        private bool createDirectory;

        public bool CreateDirectory
        {
            get { return createDirectory; }
            set
            {
                createDirectory = value;
                OnPropertyChanged("CreateDirectory");
            }
        }



        public RelayCommand BrowseNugetSpecCommand { get; set; }
        public RelayCommand BrowsePackageLocationCommand { get; set; }
        public RelayCommand BrowseNugetExeCommand { get; set; }
        public RelayCommand BrowseSquirrelExeCommand { get; set; }
        public RelayCommand BrowseReleasePathCommand { get; set; }
        public RelayCommand BrowseGenerateCommand { get; set; }

        #endregion

        #region Ctors

        public MainViewModel()
        {
            BrowseNugetSpecCommand = new RelayCommand(OnBrowseNugetSpecClicked);
            BrowsePackageLocationCommand = new RelayCommand(OnBrowsePackageLocationClicked);
            BrowseNugetExeCommand = new RelayCommand(OnBrowseNugetExeClicked);
            BrowseSquirrelExeCommand = new RelayCommand(OnBrowseSquirrelExeClicked);
            BrowseReleasePathCommand = new RelayCommand(OnBrowseReleasePathClicked);
            BrowseGenerateCommand = new RelayCommand(OnBrowseGenerateClicked);
            CreateDirectory = true;
            appSetting = new AppSetting();

            var setting = FileHelper.ReadFile("AppSettings.settings");
            if (string.IsNullOrEmpty(setting) == false)
            {
                appSetting = JsonHelper.Deserialize<AppSetting>(setting);
                NuGetExecutable  = appSetting.NugetExe;
                SquirrelExecutable  = appSetting.SquirrelExe;
            }
        }

        #endregion

        #region Methods

        private void OnBrowseGenerateClicked(object obj)
        {
            try
            {
                appSetting.NugetExe = NuGetExecutable;
                appSetting.SquirrelExe = SquirrelExecutable;
                var json = JsonHelper.Serialize(appSetting);
                FileHelper.WriteFile(json, "AppSettings.settings");

                var releasePath = ReleasePath;
                var nugetPackageDirectory = PackageFileLocaton;

                if (CreateDirectory)
                {
                    releasePath = Path.Combine(releasePath, Path.GetFileNameWithoutExtension(NuGetSpecFile.Split('.')[0]));
                    if (!Directory.Exists(releasePath))
                        Directory.CreateDirectory(releasePath);

                    var packProcess = new Process();
                    packProcess.StartInfo.FileName = NuGetExecutable;
                    packProcess.StartInfo.Arguments = " pack " + NuGetSpecFile + " -OutputDirectory " + nugetPackageDirectory;
                    packProcess.StartInfo.UseShellExecute = false;
                    packProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    packProcess.Start();
                    packProcess.WaitForExit();

                    var packageFile = Path.Combine(nugetPackageDirectory, Path.GetFileNameWithoutExtension(NuGetSpecFile) + ".nupkg");
                    var releaseProcess = new Process();
                    releaseProcess.StartInfo.FileName = SquirrelExecutable;
                    releaseProcess.StartInfo.Arguments = " --releasify " + packageFile + " --releaseDir " + releasePath;
                    releaseProcess.StartInfo.UseShellExecute = false;
                    packProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    releaseProcess.Start();
                    releaseProcess.WaitForExit();
                    Process.Start(releasePath);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void OnBrowseReleasePathClicked(object obj)
        {
            try
            {
                var fileName = "";
                var result = OpenFolderDialog("Select Release Path...", "", out fileName);
                if (result)
                    ReleasePath = fileName;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void OnBrowseSquirrelExeClicked(object obj)
        {
            try
            {
                var fileName = "";
                var result = OpenFileDialog("Select Squirrel Executable...", "Exe files (*.exe)|*.exe", out fileName);
                if (result)
                    SquirrelExecutable = fileName;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void OnBrowseNugetExeClicked(object obj)
        {
            try
            {
                var fileName = "";
                var result = OpenFileDialog("Select Nuget Executable...", "Exe files (*.exe)|*.exe", out fileName);
                if (result)
                    NuGetExecutable = fileName;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void OnBrowsePackageLocationClicked(object obj)
        {
            try
            {
                var fileName = "";
                var result = OpenFolderDialog("Select Package Location...", "", out fileName);
                if (result)
                    PackageFileLocaton = fileName;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void OnBrowseNugetSpecClicked(object obj)
        {
            try
            {
                var fileName = "";
                var result = OpenFileDialog("Select Nuget Spec...", "NuGet spec files (*.nuspec)|*.nuspec", out fileName);
                if (result)
                    NuGetSpecFile = fileName;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override Task CloseAsync()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public override bool ValidateForm()
        {
            throw new NotImplementedException();
        }

        public override Task<bool> ValidateFormAsync()
        {
            throw new NotImplementedException();
        }

        public override bool ValidatePropertyChanges(string propertyName)
        {
            return true;
        }

        #endregion
    }
}
