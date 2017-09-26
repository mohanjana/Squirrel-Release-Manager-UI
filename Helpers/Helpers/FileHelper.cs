using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Files
{
    public class FileHelper
    {
        public static bool IsDirectory(string _path)
        {
            System.IO.FileAttributes fa = System.IO.File.GetAttributes(_path);
            bool isDirectory = false;
            if ((fa & FileAttributes.Directory) != 0)
            {
                isDirectory = true;
            }
            return isDirectory;
        }

        public async static Task CopyFile(string _sourceFilePath, string _destFilePath) 
        {
            await Task.Run(() => File.Copy(_sourceFilePath, _destFilePath));
        }

        public async static Task CopyDirectory(string _sourceDirPath, string _destDirPath, bool _copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(_sourceDirPath);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + _sourceDirPath);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(_destDirPath))
            {
                Directory.CreateDirectory(_destDirPath);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(_destDirPath, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (_copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(_destDirPath, subdir.Name);
                    await Task.Run(() => CopyDirectory(subdir.FullName, temppath, _copySubDirs));
                }
            }
        }

        public async static Task DeleteFile(string _fileName)
        {
            await Task.Run(() => File.Delete(_fileName));
        }

        public async static Task DeleteDirectory(string _directoryPath) 
        {
            await Task.Run(() => Directory.Delete(_directoryPath, true));
        }

        public async static Task RenameFile(string _sourceFilePath, string _destFilePath) 
        {
            await Task.Run(() => File.Move(_sourceFilePath, _destFilePath));
        }

        public async static Task RenameDirectory(string _sourceDirPath, string _destDirPath) 
        {
            await Task.Run(() => Directory.Move(_sourceDirPath, _destDirPath));
        }

        public static List<string> GetFileList(string _dirPath, string _pattern = "*.*")
        {
            return Directory.GetFiles(_dirPath, _pattern, SearchOption.AllDirectories).ToList();
        }

        public async static Task<List<string>> GetFileListAsync(string _dirPath, string _pattern = "*.*") 
        {
            return await Task.Run(() => Directory.GetFiles(_dirPath, _pattern, SearchOption.AllDirectories).ToList());
        }

        public async static Task<long> GetDirectorySize(string _directoreName)
        {
            var fileList = Directory.GetFiles(_directoreName, "*.*");
            long fullSize = 0;
            foreach (string file in fileList)
            {
                FileInfo info = new FileInfo(file);
                await Task.Run(() => fullSize += info.Length);
            }
            return fullSize;
        }

        public static bool WriteJsonFile(string input, string fileName) 
        {
            using (var file = File.CreateText(fileName))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, input);
            }

            return true;
        }

        public static string ReadJsonFile(string fileName) 
        {
            var read = File.ReadAllText(fileName);
            var fileContent = JsonConvert.DeserializeObject<string>(read);
            return fileContent;
        }
        
        public async static Task<string> ReadJsonFileAsync(string fileName)
        {
            var read = await Task.Run(() => File.ReadAllText(fileName));
            var fileContent = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<string>(read));
            return fileContent;
        }

        public static T ReadJsonFile<T>(string fileName, T type)
        {
            var read = File.ReadAllText(fileName);
            var fileContent = JsonConvert.DeserializeObject<T>(read);
            return fileContent;
        }

        public async static Task<T> ReadJsonFileAsync<T>(string fileName, T type)
        {
            var read = File.ReadAllText(fileName);
            var fileContent = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(read));
            return fileContent;
        }
    }
}