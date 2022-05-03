namespace Bookstore.Models.Api.Books
{
    using System.Collections.Generic;

    public class AllBooksApiResponseModel
    {
        public int CurrentPage { get; set; }

        public int BooksPerPage { get; set; }

        public int TotalBooks { get; set; }

        public IEnumerable<BookResponseModel> Books { get; set; }
    }
}
