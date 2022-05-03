namespace Bookstore.Models.Api.Books
{
    public class AllBooksApiRequestModel
    {
        public string Author { get; set; }

        public string SearchTerm { get; set; }

        public BookSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int BooksPerPage { get; set; } = 10;

        public int TotalBooks { get; set; }
    }
}
