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

        public static StreamWriter CreateFile(string fileName, bool isCloseWriter = true)
        {
            var stream = File.CreateText(fileName);

            if (isCloseWriter)
            {
                stream.Close();
                stream.Dispose();
            }

            return stream;
        }

        public static List<string> GetFileList(string _dirPath, string _pattern = "*.*")
        {
            return Directory.GetFiles(_dirPath, _pattern, SearchOption.AllDirectories).ToList();
        }

        public static string ReadFile(string file)
        {
            if (File.Exists(file))
            {
                var line = "";
                using (var sr = new StreamReader(file))
                {
                    line = sr.ReadToEnd();
                    sr.Close();
                }
                return line;
            }
            else
                return null;
        }

        public static void WrirteFile(string input, string fileName)
        {
            using (var writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(input);
                writer.Close();
            }
        }

        public async static Task CopyFileAsync(string _sourceFilePath, string _destFilePath, bool overWrite = false)
        {
            await Task.Run(() => File.Copy(_sourceFilePath, _destFilePath, overWrite));
        }

        public async static Task CopyDirectoryAsync(string _sourceDirPath, string _destDirPath, bool _copySubDirs)
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
                    await Task.Run(() => CopyDirectoryAsync(subdir.FullName, temppath, _copySubDirs));
                }
            }
        }

        public async static Task DeleteFileAsync(string _fileName)
        {
            await Task.Run(() => File.Delete(_fileName));
        }

        public async static Task DeleteDirectoryAsync(string _directoryPath)
        {
            await Task.Run(() => Directory.Delete(_directoryPath, true));
        }

        public async static Task RenameFileAsync(string _sourceFilePath, string _destFilePath)
        {
            await Task.Run(() => File.Move(_sourceFilePath, _destFilePath));
        }

        public static bool ExistsFile(string fileName)
        {
            return File.Exists(fileName);
        }

        public async static Task RenameDirectoryAsync(string _sourceDirPath, string _destDirPath)
        {
            await Task.Run(() => Directory.Move(_sourceDirPath, _destDirPath));
        }

        public async static Task<List<string>> GetFileListAsync(string _dirPath, string _pattern = "*.*")
        {
            return await Task.Run(() => Directory.GetFiles(_dirPath, _pattern, SearchOption.AllDirectories).ToList());
        }

        public async static Task<long> GetDirectorySizeAsync(string _directoreName)
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

        public async static Task<string> ReadFileAsync(string file)
        {
            string readContents;
            using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
            {
                readContents = await streamReader.ReadToEndAsync();
                streamReader.Close();
            }
            return readContents;
        }

        public async static Task WrirteFileAsync(string input, string fileName)
        {
            using (var writer = new StreamWriter(fileName, true))
            {
                await writer.WriteLineAsync(input);
                writer.Close();
            }
        }

        public static void WriteFile(string text, string path)
        {
            if (!File.Exists(path))
                using (var sw = File.CreateText(path))
                    sw.Close();
            else
                return;

            using (var sw = File.AppendText(path))
            {
                sw.WriteLine(text);
                sw.Close();
            }
        }
    }
}
