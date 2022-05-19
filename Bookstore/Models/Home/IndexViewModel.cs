namespace Bookstore.Models.Home
{
    using Bookstore.Services.Books.Models;
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int TotalBooks { get; set; }

        public int TotalUsers { get; set; } 

        public int TotalRents { get; set; }

        public IList<LatestBooksServiceModel> Books { get; set; }

    }
}
