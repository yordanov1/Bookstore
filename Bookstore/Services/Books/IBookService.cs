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


        BookDetailsServiceModel Details(int bookId);

        int Create(
                string bookTitle,
                string author,
                string imageUrl,
                string publishingHouse,
                int? rating,
                string description,
                int genreId,
                int moderatorId);

        bool Edit(
              int bookId,
              string bookTitle,
              string author,
              string imageUrl,
              string publishingHouse,
              int? rating,
              string description,
              int genreId);

        IEnumerable<BookServiceModel> ByUser(string userId);

        bool IsByModerator(int bookId, int moderatorId);

        IEnumerable<string> AllBookAuthors();


        IEnumerable<BookGenreServiceModel> AllBookGenres();

        bool GenreExist(int genreId);

    }
}
    