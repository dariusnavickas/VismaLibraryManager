using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using BusinessLayer;

namespace DataLayer
{
    public class Data : IData
    {
        private string _filename;

        public Data(string filename)
        {
            _filename = filename;
        }

        public List<Book> Load()
        {
            if (!File.Exists(_filename)) File.Create(_filename).Dispose();
            string jsonData = File.ReadAllText(_filename);

            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(jsonData);
            if (books == null) books = new List<Book>();
            return books;
        }

        public void Save(List<Book> books)
        {
            var jsonData = JsonConvert.SerializeObject(books, Formatting.Indented);
            File.WriteAllText(_filename, jsonData);
        }
    }
}
