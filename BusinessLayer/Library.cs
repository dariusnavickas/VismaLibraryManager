using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class Library : ILibrary
    {
        public const int MaxBookLoanCount = 3;
        public const int MaxLoanMonthCount = 2;

        private readonly List<Book> _books;
        private readonly IData _data;

        private readonly string bookDoesNotExist = "Book does not exist.";
        private readonly string bookNotLoanedToReader = "Book was not loaned to the reader.";
        private readonly string nullOrWhiteSpace = "Book cannot have null of empty value.";
        private readonly string bookExistsWithSameIsbn = "Book already exists with the same ISBN";
        private readonly string allCopiesAreLoaned = "All copies are loaned.";
        private readonly string bookNotAvailable = "Book is not available.";
        private readonly string loanLimitReached = "The reader already reached the loans limit";
        private readonly string returnTimeLimitReached = $"Can't borrow book for more than {MaxLoanMonthCount} months";
        private readonly string dateInFuture = "Return have to be today or in future";


        public Library(IData data)
        {
            _data = data;
            _books = _data.Load();
        }

        public void AddBook(string isbn)
        {
            if (!DoesBookExist(isbn))
                throw new BookDoesNotExistException(bookDoesNotExist); ;

            AddDuplicateBook(isbn);
        }

        public void AddBook(
            string name, string author, string category, 
            string language, ushort year, string isbn)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(author) ||
                string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(language) ||
                string.IsNullOrWhiteSpace(isbn))
                throw new BookCannotHaveNullOrWhiteSpaceValuesException(nullOrWhiteSpace);

            if (DoesBookExist(isbn))
                throw new BookAlreadyExistsWithTheSameIsbnException(bookExistsWithSameIsbn);

            _books.Add(new Book(name, author, category, language, year, isbn)
            {
                Name = name,
                Author = author,
                Category = category,
                Language = language,
                PublicationYear = year,
                ISBN = isbn
            });
            _data.Save(_books);
        }

        public void DeleteBook(string isbn, string readerId)
        {
            if (!DoesBookExist(isbn)) 
                throw new BookDoesNotExistException(bookDoesNotExist);
            Book book = GetLoanedBook(isbn, readerId);
            if (book == null) 
                throw new BookIsNotLoanedToTheReaderException(bookNotLoanedToReader);
            
            _books.Remove(book);
            _data.Save(_books);
        }

        public void DeleteBook(string isbn)
        {
            if (!DoesBookExist(isbn)) 
                throw new BookDoesNotExistException(bookDoesNotExist);
            Book book = GetAvailableBook(isbn);
            if (book == null) 
                throw new BookIsNotAvailableException(allCopiesAreLoaned);
            
            _books.Remove(book);
            _data.Save(_books);
        }

        public List<Book> ListBooks(
            string author, string category, string language, 
            string isbn, string name, bool byAvailability, bool isTaken)
        {
            List<Book> filteredBooks = new(_books);
            filteredBooks = filteredBooks.
                Where(b => b.Author.ToLower().Contains(author.ToLower().Trim())).ToList().
                Where(b => b.Category.ToLower().Contains(category.ToLower().Trim())).ToList().
                Where(b => b.Language.ToLower().Contains(language.ToLower().Trim())).ToList().
                Where(b => b.ISBN.ToLower().Contains(isbn.ToLower().Trim())).ToList().
                Where(b => b.Name.ToLower().Contains(name.ToLower().Trim())).ToList();

            if (!byAvailability) return filteredBooks;
            if (isTaken) return filteredBooks.Where(b => b.IsTaken).ToList();
            return filteredBooks.Where(b => !b.IsTaken).ToList();
        }

        public List<Book> ListBooks()
        {
            return new List<Book>(_books);
        }

        public bool ReturnBook(string isbn, string readerId)
        {
            if (!DoesBookExist(isbn)) 
                throw new BookDoesNotExistException(bookDoesNotExist);
            Book book = GetLoanedBook(isbn, readerId);
            if (book == null) 
                throw new BookIsNotLoanedToTheReaderException(bookNotLoanedToReader);

            bool isLateReturn = !IsLateReturn(isbn, readerId);
            book.IsTaken = false;
            book.ReaderId = null;
            _data.Save(_books);

            return isLateReturn;
        }

        public void TakeBook(string isbn, string readerId, DateTime returnDate)
        {
            if (!DoesBookExist(isbn)) 
                throw new BookDoesNotExistException(bookDoesNotExist);
            Book book = GetAvailableBook(isbn);
            if (book == null)
                throw new NoAvailableBooksException(bookNotAvailable);
            if (IsLoanLimitReched(readerId))
                throw new ReaderReachedTheLoanLimitException(loanLimitReached);
            if (IsMoreThanAllowedLoanTime(returnDate))
                throw new LoanTimeLimitExceededException(returnTimeLimitReached);
            if (returnDate.Date < DateTime.Now.Date)
                throw new ReturnDateIsInPastException(dateInFuture);
            
            book.ReaderId = readerId;
            book.ReturnDate = returnDate;
            book.IsTaken = true;
            _data.Save(_books);
        }

        private void AddDuplicateBook(string isbn)
        {
            Book book = GetBook(isbn);
            _books.Add(new Book(book));
            _data.Save(_books);
        }

        private bool DoesBookExist(string isbn) => 
            _books
                .Exists(b => 
                    b.ISBN == isbn);

        private bool IsLoanLimitReched(string readerId) =>
            _books
                .Count(b => 
                    b.ReaderId != null && 
                    b.ReaderId == readerId) >= MaxBookLoanCount;

        private bool IsMoreThanAllowedLoanTime(DateTime date) => 
            DateTime.Now.AddMonths(MaxLoanMonthCount) < date;

        private bool IsLateReturn(string isbn, string readerId) =>
            _books
                .Exists(b => 
                    b.ISBN == isbn &&
                    b.ReaderId == readerId &&
                    b.ReturnDate.Date < DateTime.Now.Date);

        private Book GetBook(string isbn) => 
            _books
                .FirstOrDefault(b => 
                    b.ISBN.Equals(isbn));
        
        private Book GetAvailableBook(string isbn) =>
                _books
            .FirstOrDefault(b => 
                b.ISBN == isbn && 
                b.IsTaken == false);

        private Book GetLoanedBook(string isbn, string readerId) =>
            _books
                .OrderBy(x => x.ReturnDate)
                .FirstOrDefault(b =>
                    b.ISBN == isbn &&
                    b.ReaderId == readerId);
    }
}