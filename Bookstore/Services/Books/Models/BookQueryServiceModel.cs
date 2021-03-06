namespace Bookstore.Services.Books.Models
{
    using System.Collections.Generic;

    public class BookQueryServiceModel
    {
        public int CurrentPage { get; set; }

        public int BooksPerPage { get; set; }

        public int TotalBooks { get; set; }

        public IEnumerable<BookServiceModel> Books { get; set; }
    }
}
