namespace Bookstore.Controllers
{
    using Bookstore.Data;
    using Bookstore.Models;
    using Bookstore.Models.Home;
    using Bookstore.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly BookstoreDbContext data;
        private readonly IStatisticServices statistics;

        public HomeController(
            BookstoreDbContext data, 
            IStatisticServices statistics)
        {
            this.statistics = statistics;
            this.data = data;
        }

        public IActionResult Index()
        {
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
                 //Genre = book.Genre.Name
             })
             .Take(3)
             .ToList();
            
            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalBooks = totalStatistics.TotalBooks,
                TotalUsers = totalStatistics.TotalUsers,
                Books = books
            });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
