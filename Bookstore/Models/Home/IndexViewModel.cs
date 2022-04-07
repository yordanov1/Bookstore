namespace Bookstore.Models.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalUsers { get; set; } 
        public int TotalRents { get; set; }

        public List<BookIndexViewModel> Books { get; set; }

    }
}
