namespace Bookstore.Controllers
{
    using Bookstore.Models.Home;
    using Bookstore.Services.Books;
    using Bookstore.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
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

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalBooks = totalStatistics.TotalBooks,
                TotalUsers = totalStatistics.TotalUsers,
                TotalModerators = totalStatistics.TotalModerators,
                Books = latestBooks
            });
        }
    }
}
