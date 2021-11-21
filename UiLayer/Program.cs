using BusinessLayer;
using ConsoleTables;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Threading;

namespace UiLayer
{
    class Program
    {
        private static Library library;

        private static string filename = "Books.json";

        private static void PrintCommands()
        {
            Console.WriteLine(
                "Please enter one the following commands:\n" +
                "" +
                "\tadd Name//Author//Category//Language//PublicationYear//ISBN\n" +
                "\t\t-Book with the same ISBN should not exist in the library\n" +
                "\t\t-Publication year should be a number\n" +
                "\t\t-No field should be empty\n" +
                "" +
                "\tadd ISBN\n" +
                "\t\t-There should be a book in the library with the same ISBN\n" +
                "" +
                "\ttake ISBN//ReaderID//ReturnDate\n" +
                "\t\t-Book with ISBN should be in the library and available\n" +
                $"\t\t-Return date cannot be further than {Library.MaxLoanMonthCount} months away or in the past\n" +
                $"\t\t-Specified reader should not currenty have {Library.MaxBookLoanCount} books taken\n" +
                "" +
                "\treturn ISBN//ReaderID\n" +
                "\t\t-Book with ISBN should be registered with the library and taken by the reader\n" +
                "" +
                "\tdelete ISBN\n" +
                "\t\t-Book with ISBN should be in the library and available\n" +
                "" +
                "\tdelete ISBN//ReaderID\n" +
                "\t\t-Book with ISBN shoudl be taken by the reader\n" +
                "" +
                "\tlist Name//Author//Category//Language//ISBN//Availability\n" +
                "\t\t-Fields are used filter books\n" +
                "\t\t-Use space between // to leave value empty\n" +
                "\t\t-Availability must be supplied be\n" +
                "\t\t\tt -For taken\n" +
                "\t\t\ta -For available\n" +
                "\t\t\tb -For taken and available\n" +
                "" +
                "\tlist -To list all books without filters\n" +
                "" +
                "\tcommands -To list commands\n" +
                "" +
                "\texit -To exit");
        }

        static void Main(string[] args)
        {
            var data = new Data(filename);
            library = new Library(data);

            bool exit = false;
            Console.WriteLine("Welcome to Visma's library");
            PrintCommands();
            while (!exit)
            {
                Console.WriteLine("Enter a command:");
                string command = Console.ReadLine();
                if (command.Split(' ').Length > 1 || 
                    IsSingleWordCommand(command))
                {
                    string subcommand;
                    if (IsSingleWordCommand(command))
                    {
                        subcommand = "";
                    }
                    else
                    {
                        subcommand = command.Substring(command.Split(' ')[0].Length+1);
                    }

                    try
                    {
                        switch (command.Split(' ')[0])
                        {
                            case "add":
                                Add(subcommand);
                                break;
                            case "take":
                                Take(subcommand);
                                break;
                            case "return":
                                Return(subcommand);
                                break;
                            case "delete":
                                Delete(subcommand);
                                break;
                            case "list":
                                List(subcommand);
                                break;
                            case "commands":
                                PrintCommands();
                                break;
                            case "exit":
                                exit = true;
                                break;
                            default:
                                PrintError();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"ERROR: {ex.Message}\n" +
                            $"Please try again");
                    }
                }
                else
                {
                    PrintError();
                }
            }
        }

        private static bool IsSingleWordCommand(string command)
        {
            return command == "list" || command == "exit" || command == "commands";
        }

        private static void PrintError()
        {
            Console.WriteLine("Command was not recognised");
        }

        private static void Add(string subcommand)
        {
            string[] subcommands = subcommand.Split("//");
            switch (subcommands.Length)
            {
                case 1:
                    library.AddBook(subcommands[0]);
                    Console.WriteLine("Book added to the library");
                    break;
                case 6:
                    if (ushort.TryParse(subcommands[4], out ushort year))
                    {
                        library.AddBook(subcommands[0], subcommands[1], subcommands[2], subcommands[3], year, subcommands[5]);
                        Console.WriteLine("Book added to the library");
                        break;
                    }
                    Console.WriteLine("Year have to be a number: 0 - 65535");
                    Console.WriteLine("Book NOT added to the library");
                    break;
                default:
                    PrintError();
                    break;
            }
        }

        private static void Take(string subcommand)
        {
            string[] subcommands = subcommand.Split("//");
            switch (subcommands.Length)
            {
                case 3:
                    if (DateTime.TryParse(subcommands[2], out DateTime returnDate))
                    {
                        library.TakeBook(subcommands[0], subcommands[1], returnDate);
                        Console.WriteLine("Book taken from the library");
                        break;
                    }
                    Console.WriteLine("Date was not recognised");
                    Console.WriteLine("Book not taken from the library");
                    break;
                default:
                    PrintError();
                    break;
            }
        }

        private static void Return(string subcommand)
        {
            string[] subcommands = subcommand.Split("//");
            switch (subcommands.Length)
            {
                case 2:
                    if (!library.ReturnBook(subcommands[0], subcommands[1]))
                    {
                        Console.WriteLine("We have got your return, although a little too late......");
                        for (int i = 0; i < 5; i++)
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine("SHAME!!!!....");
                        }
                        return;
                    }
                    Console.WriteLine("Book returned to the library");
                    break;
                default:
                    PrintError();
                    break;
            }
        }

        private static void Delete(string subcommand)
        {
            string[] subcommands = subcommand.Split("//");
            switch (subcommands.Length)
            {
                case 1:
                    library.DeleteBook(subcommands[0]);
                    Console.WriteLine("Book deleted");
                    break;
                case 2:
                    library.DeleteBook(subcommands[0], subcommands[1]);
                    Console.WriteLine("Book deleted");
                    break;
                default:
                    PrintError();
                    break;
            }
        }
        
        private static void List(string subcommand)
        {
            string[] subcommands = subcommand.Split("//");
            switch (subcommands.Length)
            {
                case 1:
                    PrintConsoleTable(library.ListBooks());
                    break;
                case 6:
                    bool filterByAvailability;
                    bool isTaken;
                    switch (subcommands[5])
                    {
                        case "t":
                            filterByAvailability = true;
                            isTaken = true;
                            break;
                        case "a":
                            filterByAvailability = true;
                            isTaken = false;
                            break;
                        case "b":
                            filterByAvailability = false;
                            isTaken = false;
                            break;
                        default:
                            PrintError();
                            return;
                    }
                    PrintConsoleTable(library
                        .ListBooks(subcommands[0], subcommands[1], 
                        subcommands[2], subcommands[3], subcommands[4], 
                        filterByAvailability, isTaken));
                    break;
                default:
                    PrintError();
                    break;
            }
        }

        private static void PrintConsoleTable(List<Book> books)
        {
            ConsoleTable consoleTable = new ConsoleTable("Name", "Author", "Category", "Language", "ISBN", "Available", "Reader ID", "Return Date");

            string readerId;
            string isTaken;
            string returnDate;
            foreach (Book book in books)
            {
                readerId = book.ReaderId;
                isTaken = "NO";
                returnDate = book.ReturnDate.ToShortDateString();

                if (readerId == null)
                {
                    readerId = "N/A";
                    returnDate = "N/A";
                    isTaken = "YES";
                }
                consoleTable.AddRow(book.Name, book.Author, book.Category, book.Language, book.ISBN, isTaken, readerId, returnDate);
            }
            consoleTable.Write();
            Console.WriteLine();
        }
    }
}