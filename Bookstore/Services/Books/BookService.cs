namespace Bookstore.Services.Books
{
    using Bookstore.Data;
    using Bookstore.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class BookService : IBookService
    {
        private readonly BookstoreDbContext data;

        public BookService(BookstoreDbContext data)
        {
            this.data = data;
        }

        public BookQueryServiceModel All(
            string author, 
            string searchTerm,
            BookSorting sorting,
            int currentPage,
            int booksPerPage)
        {
            var booksQuery = this.data.Books.AsQueryable();


            if (!string.IsNullOrWhiteSpace(author))
            {
                booksQuery = booksQuery.Where(x => x.Author == author);
            }


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                booksQuery = booksQuery.Where(x =>
                       (x.BookTitle + " " + x.Author).ToLower().Contains(searchTerm.ToLower())
                     || x.Description.ToLower().Contains(searchTerm.ToLower()));
            }


            booksQuery = sorting switch
            {
                BookSorting.Rating => booksQuery.OrderByDescending(x => x.Rating),
                BookSorting.Author => booksQuery.OrderByDescending(x => x.Author),
                _ => booksQuery.OrderByDescending(x => x.Id)
            };


            var totalBooks = booksQuery.Count();


            var books = booksQuery
                .Skip((currentPage - 1) * booksPerPage)
                .Take(booksPerPage)
                .Select(book => new BookServiceModel
                {
                    Id = book.Id,
                    BookTitle = book.BookTitle,
                    Author = book.Author,
                    ImageUrl = book.ImageUrl,
                    PublishingHouse = book.PublishingHouse,
                    Rating = book.Rating,
                    Description = book.Description,
                    Genre = book.Genre.Name
                })
                .ToList();

            return new BookQueryServiceModel
            {
                TotalBooks = totalBooks,
                CurrentPage = currentPage,
                BooksPerPage = booksPerPage,
                Books = books
            };
        }

        public IEnumerable<string> AllBookAuthors()
        {
            return this.data
                .Books
                .Select(x => x.Author)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
        }
    }
}
