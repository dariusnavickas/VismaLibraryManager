using Newtonsoft.Json;
using System;

namespace BusinessLayer
{
    public class Book
    {
        [JsonConstructor]
        public Book(string name, string author, string category, string language, ushort publicationYear, string isbn)
        {
            Name = name;
            Author = author;
            Category = category;
            Language = language;
            PublicationYear = publicationYear;
            ISBN = isbn;
            IsTaken = false;
            ReaderId = null;
            ReturnDate = new DateTime();
        }

        public Book(Book book)
        {
            Name = book.Name;
            Author = book.Author;
            Category = book.Category;
            Language = book.Language;
            PublicationYear = book.PublicationYear;
            ISBN = book.ISBN;
            IsTaken = false;
            ReaderId = null;
            ReturnDate = new DateTime();
        }

        public string Name { get; }

        public string Author { get; }

        public string Category { get; }

        public string Language { get; }

        public ushort PublicationYear { get; }

        public string ISBN { get; }

        public bool IsTaken { get; set; }

        public string ReaderId { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}
