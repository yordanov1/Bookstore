namespace Bookstore.Controllers
{
    using Bookstore.Models;
    using Bookstore.Models.Home;
    using Bookstore.Services.Books;
    using Bookstore.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly IBookService books;
        private readonly IStatisticServices statistics;
        

        public HomeController(
            IStatisticServices statistics,
            IBookService books)
        {
            this.statistics = statistics;            
            this.books = books;
        }

        public IActionResult Index()
        {

            var latestBooks = this.books
                .Latest()
                .ToList();


            /*
            var books = this.data
             .Books
             .OrderByDescending(x => x.Id)
             .Select(book => new BookIndexViewModel
             {
                 Id = book.Id,
                 BookTitle = book.BookTitle,
                 Author = book.Author,
                 ImageUrl = book.ImageUrl,
                 PublishingHouse = book.PublishingHouse,
                 Rating = book.Rating,
                 Description = book.Description,
             })
             .Take(3)
             .ToList();
            */


            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalBooks = totalStatistics.TotalBooks,
                TotalUsers = totalStatistics.TotalUsers,
                Books = latestBooks
            });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
