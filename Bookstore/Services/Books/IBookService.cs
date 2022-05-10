namespace Bookstore.Services.Books
{
    using Bookstore.Models;
    using System.Collections.Generic;

    public interface IBookService
    {
        BookQueryServiceModel All(
            string author,
            string searchTerm,
            BookSorting sorting,
            int currentPage,
            int booksPerPage);


        IEnumerable<BookServiceModel> ByUser(string userId);

        IEnumerable<string> AllBookAuthors();
    }
}
    