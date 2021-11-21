using Newtonsoft.Json;
using System;

namespace BusinessLayer
{
    public class Book
    {
        string _name;
        string _author;
        string _category;
        string _language;
        ushort _publicationYear;
        string _isbn;

        bool _isTaken;
        string _readerId;
        DateTime _returnDate;

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

        public string Name
        {
            get => _name;
            init
            {
                if (!string.IsNullOrWhiteSpace(value)) _name = value;
            }
        }

        public string Author
        {
            get => _author;
            init
            {
                if (!string.IsNullOrWhiteSpace(value)) _author = value;
            }
        }

        public string Category
        {
            get => _category;
            init
            {
                if (!string.IsNullOrWhiteSpace(value)) _category = value;
            }
        }

        public string Language
        {
            get => _language;
            init
            {
                if (!string.IsNullOrWhiteSpace(value)) _language = value;
            }
        }

        public ushort PublicationYear
        {
            get => _publicationYear;
            init
            {
                _publicationYear = value;
            }
        }

        public string ISBN
        {
            get => _isbn;
            init
            {
                if (!string.IsNullOrWhiteSpace(value)) _isbn = value;
            }
        }

        public bool IsTaken
        {
            get => _isTaken;
            set
            {
                _isTaken = value;
            }
        }

        public string ReaderId
        {
            get => _readerId;
            set
            {
                _readerId = value;
            }
        }

        public DateTime ReturnDate
        {
            get => _returnDate;
            set
            {
                _returnDate = value;
            }
        }
    }
}
