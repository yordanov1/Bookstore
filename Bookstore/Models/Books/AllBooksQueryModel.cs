namespace Bookstore.Models.Books
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllBooksQueryModel
    {
        public string Author { get; set; }
        public IEnumerable<string> Authors { get; set; }

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public BookSorting Sorting { get; set; }
        public IEnumerable<BookListingViewModel> Books { get; set; } 
    }
}
