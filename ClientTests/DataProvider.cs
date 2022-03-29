using View.Entries;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace ClientTests
{
    internal static class DataProvider
    {
        public static List<EntryCreateInfo> GetData()
        {
            var directory = new DirectoryInfo("Data");
            var files = directory.GetFiles();
            var data = new List<EntryCreateInfo>();

            foreach (var file in files)
            {
                using var streamReader = new StreamReader(file.OpenRead());
                var content = streamReader.ReadToEnd();

                var entry = JsonSerializer.Deserialize<EntryCreateInfo>(content);
                data.Add(entry);
            }

            return data;
        }
    }
}
