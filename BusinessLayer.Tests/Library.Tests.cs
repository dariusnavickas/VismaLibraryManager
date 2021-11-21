using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace BusinessLayer.Tests
{
    public class LibraryTests
    {
        readonly Library library;
        readonly Mock<IData> dataMock;

        public LibraryTests()
        {
            dataMock = new Mock<IData>();
            dataMock.Setup(x => x.Load()).Returns(new List<Book>
            {
                new Book("Clean Code", "Martin", "Soft", "EN", 2008, "111"),
                new Book("Clean Coder", "Martin", "Soft", "EN", 2011, "222"),
                new Book("Pragmatic Programmer", "Andrew Hunt", "Soft", "EN", 2019, "333"),
                new Book("Art of Unit Testing", "Osherove", "Test", "EN", 2013, "444")
        });
            library = new Library(dataMock.Object);
        }

        [Fact]
        public void AddBookShouldAddABookDuplicateBook()
        {
            library.AddBook("111");
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Once);
            Assert.Equal(2, library.ListBooks().Count(b => b.ISBN == "111"));
        }

        [Fact]
        public void AddBookShouldThrowBookDoesNotExistException()
        {
            Assert.Throws<BookDoesNotExistException>(() => library.AddBook("555"));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }

        [Fact]
        public void AddBookShouldAddABook()
        {
            library.AddBook("Crystal Clear", "Cockburn", "Engineering", "English", 2020, "555");
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Once);
            Assert.Contains(library.ListBooks(), b => b.ISBN == "555");
        }

        [Theory]
        [InlineData("", "", "", "", "")]
        [InlineData("", "author", "cat", "lang", "isbn")]
        [InlineData("book", "", "cat", "lang", "isbn")]
        [InlineData("book", "author", "", "lang", "isbn")]
        [InlineData("book", "author", "cat", "", "isbn")]
        [InlineData("book", "author", "cat", "lang", "")]
        public void AddBookShouldThrowBookCannotHaveNullOrWhiteSpaceValuesException(string name, string author, string category, string language, string isbn)
        {
            Assert.Throws<BookCannotHaveNullOrWhiteSpaceValuesException>(() => library.AddBook(name, author, category, language, 2020, isbn));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }
        
        [Fact]
        public void AddBookShouldThrowBookAlreadyExistsWithTheSameIsbnException()
        {
            Assert.Throws<BookAlreadyExistsWithTheSameIsbnException>(() => 
                library.AddBook("Crystal Clear", "Cockburn", "Engineering", "English", 2020, "111"));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }

        [Fact]
        public void DeletaBookShouldDeleteABook()
        {
            library.DeleteBook("111");
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Once);
            Assert.DoesNotContain(library.ListBooks(), b => b.ISBN == "111");
        }
        
        [Fact]
        public void DeleteBookShouldThrowBookDoesNotExistException()
        {
            Assert.Throws<BookDoesNotExistException>(() =>
                library.DeleteBook("555"));
            Assert.Throws<BookDoesNotExistException>(() =>
                library.DeleteBook("555", "1"));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }
        
        [Fact]
        public void DeleteBookShouldThrowBookIsNotAvailableException()
        {
            library.TakeBook("111", "1", DateTime.Now.AddDays(1).Date);
            Assert.Throws<BookIsNotAvailableException>(() =>
                library.DeleteBook("111"));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Once);
        }

        [Fact]
        public void DeletaBookShouldDeleteATakenBook()
        {
            library.TakeBook("111", "1", DateTime.Now.AddDays(1).Date);
            library.DeleteBook("111", "1");
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Exactly(2));
            Assert.DoesNotContain(library.ListBooks(), b => b.ISBN == "111");
        }

        [Fact]
        public void DeleteBookShouldBookIsNotLoanedToTheReaderException()
        {
            Assert.Throws<BookIsNotLoanedToTheReaderException>(() =>
                library.DeleteBook("111", "1"));
            library.TakeBook("111", "2", DateTime.Now.AddDays(1).Date);
            Assert.Throws<BookIsNotLoanedToTheReaderException>(() =>
                library.DeleteBook("111", "1"));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Once);
        }

        [Theory]
        [InlineData("", "", "EN", "111", "", true, false, 0)]
        [InlineData("", "", "EN", "111", "", true, true, 1)]
        [InlineData("", "", "EN", "", "", true, false, 1)]
        [InlineData("marTIN", "", "", "", "COD", true, true, 2)]
        [InlineData("", "", "", "", "", true, true, 3)]
        [InlineData("", "oft", "", "", "", false, true, 3)]
        public void ListBooksShouldListFilteredBooks(
            string author, string category, string language, string isbn, 
            string name, bool byAvailability, bool byIsTaken, int expectedCount)
        {
            library.TakeBook("111", "2", DateTime.Now.AddDays(1).Date);
            library.TakeBook("222", "2", DateTime.Now.AddDays(15).Date);
            library.TakeBook("333", "1", DateTime.Now.AddDays(7).Date);
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Exactly(3));

            Assert.Equal(expectedCount, library.ListBooks(author, category, language, isbn, name, byAvailability, byIsTaken).Count);
        }

        [Fact]
        public void ListBooksShouldListAllBooks()
        {
            Assert.Equal(4, library.ListBooks().Count);
            library.AddBook("111");
            library.AddBook("222");
            library.AddBook("333");
            library.AddBook("222");
            Assert.Equal(8, library.ListBooks().Count);
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Exactly(4));
        }

        [Fact]
        public void ReturnBookShouldReturnABook()
        {
            library.TakeBook("111", "2", DateTime.Now.AddDays(1).Date);
            library.ReturnBook("111", "2");
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Exactly(2));
        }

        [Fact]
        public void ReturnBookShouldReturnTrueWhenBookIsReturnedOnTime()
        {
            library.TakeBook("111", "2", DateTime.Now.AddDays(1).Date);
            Assert.True(library.ReturnBook("111", "2"));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Exactly(2));
        }

        [Fact]
        public void ReturnBookShouldSetIsTakenToFalseAndReaderToNullOnReturnedBook()
        {
            library.TakeBook("111", "2", DateTime.Now.AddDays(1).Date);
            library.ReturnBook("111", "2");
            Assert.Contains(library.ListBooks(), b => 
                b.ISBN == "111" && 
                b.ReaderId == null && 
                b.IsTaken == false);
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Exactly(2));
        }

        [Fact]
        public void ReturnBookShouldThrowBookDoesNotExistException()
        {
            Assert.Throws<BookDoesNotExistException>(() =>
                library.ReturnBook("555", "2"));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }

        [Fact]
        public void ReturnBookShouldThrowBookIsNotLoanedToTheReaderException()
        {
            Assert.Throws<BookIsNotLoanedToTheReaderException>(() =>
                library.ReturnBook("111", "2"));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }

        [Fact]
        public void TakeBookShouldGiveBookToReader()
        {
            library.TakeBook("111", "1", DateTime.Now.AddDays(1));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Once);
        }

        [Fact]
        public void TakeBookShouldSetReaderIdReturnDateAndIsTaken()
        {
            library.TakeBook("111", "1", DateTime.Now.AddDays(1).Date);
            Assert.Contains(library.ListBooks(), b => 
                b.ReaderId == "1" && 
                b.IsTaken == true && 
                b.ReturnDate == DateTime.Now.AddDays(1).Date);
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Once);
        }

        [Fact]
        public void TakeBookShouldThrowBookDoesNotExistException()
        {
            Assert.Throws<BookDoesNotExistException>(() =>
                library.TakeBook("555", "1", DateTime.Now.AddDays(1).Date));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }

        [Fact]
        public void TakeBookShouldThrowNoAvailableBooksException()
        {
            library.TakeBook("111", "1", DateTime.Now.AddDays(1).Date);
            Assert.Throws<NoAvailableBooksException>(() =>
                library.TakeBook("111", "1", DateTime.Now.AddDays(1).Date));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Once);
        }

        [Fact]
        public void TakeBookShouldThrowReaderReachedTheLoanLimitException()
        {
            library.TakeBook("111", "1", DateTime.Now.AddDays(1).Date);
            library.TakeBook("222", "1", DateTime.Now.AddDays(1).Date);
            library.TakeBook("333", "1", DateTime.Now.AddDays(1).Date);
            Assert.Throws<ReaderReachedTheLoanLimitException>(() =>
                library.TakeBook("444", "1", DateTime.Now.AddDays(1).Date));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Exactly(3));
        }

        [Fact]
        public void TakeBookShouldThrowLoanTimeLimitExceededException()
        {
            Assert.Throws<LoanTimeLimitExceededException>(() =>
                library.TakeBook("111", "1", DateTime.Now.AddMonths(3).Date));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }

        [Fact]
        public void TakeBookShouldThrowReturnDateIsInPastException()
        {
            Assert.Throws<ReturnDateIsInPastException>(() =>
                library.TakeBook("111", "1", DateTime.Now.AddDays(-1).Date));
            dataMock.Verify(
                x => x.Save(It.IsAny<List<Book>>()),
                Times.Never);
        }

    }
}
