using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BusinessLayer
{
    interface ILibrary
    {
        public void AddBook(string isbn);

        public void AddBook(string name, string author, string category, string language, ushort year, string isbn);

        public void TakeBook(string isbn, string readerId, DateTime returnDate);

        public bool ReturnBook(string isbn, string readerId);

        public List<Book> ListBooks(string name, string author, string category, string language, string isbn, bool availability, bool isTaken);

        public List<Book> ListBooks();

        public void DeleteBook(string isbn, string readerId);

        public void DeleteBook(string isbn);
    }
}
