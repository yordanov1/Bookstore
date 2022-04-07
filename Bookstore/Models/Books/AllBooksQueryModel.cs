namespace Bookstore.Models.Books
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllBooksQueryModel
    {
        public IEnumerable<string> BookTitle { get; set; }
        [Display(Name = "Search")]
        public IEnumerable<string> SearchTerm { get; set; }
        public BookSorting Sorting { get; set; }
        public IEnumerable<BookListingViewModel> Books { get; set; } 
    }
}
