using Helpers.Files;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Helpers.Json
{
    public class JsonHelper
    {
        public static string ReadJsonFile(string fileName)
        {
            var read = FileHelper.ReadFile(fileName);
            var fileContent = JsonConvert.DeserializeObject<string>(read);
            return fileContent;
        }

        public async static Task<string> ReadJsonFileAsync(string fileName)
        {
            var read = await FileHelper.ReadFileAsync(fileName);
            var fileContent = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<string>(read));
            return fileContent;
        }

        public static T ReadJsonFile<T>(string fileName)
        {
            var read = FileHelper.ReadFile(fileName);
            var fileContent = JsonConvert.DeserializeObject<T>(read);
            return fileContent;
        }

        public async static Task<T> ReadJsonFileAsync<T>(string fileName)
        {
            var read = await FileHelper.ReadFileAsync(fileName);
            var fileContent = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(read));
            return fileContent;
        }

        public static bool WriteJsonFile(string input, string fileName)
        {
            using (var file = FileHelper.CreateFile(fileName))
            {
                FileHelper.WrirteFile(input, fileName);
            }

            return true;
        }

        public static bool WriteJsonFile<T>(T input, string fileName)
        {
            using (var file = FileHelper.CreateFile(fileName, false))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, input);
                file.Close();
            }

            return true;
        }

        public static string SerializeObject(object serializeObject)
        {
            return JsonConvert.SerializeObject(serializeObject);
        }

        public static string SerializeObject<T>(T serializeObject)
        {
            var jsonString = JsonConvert.SerializeObject(serializeObject);
            return jsonString;
        }

        public static string Serialize<T>(T entity) where T : new()
        {
            string json = JsonConvert.SerializeObject(entity);
            return json;
        }

        public static T Deserialize<T>(string json) where T : new()
        {
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
    }
}
