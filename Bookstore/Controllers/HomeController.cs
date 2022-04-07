namespace Bookstore.Controllers
{
    using Bookstore.Data;
    using Bookstore.Models;
    using Bookstore.Models.Books;
    using Bookstore.Models.Home;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : Controller
    {

        // Injektirame data
        private readonly BookstoreDbContext data;
        public HomeController(BookstoreDbContext data)
            => this.data = data;



        public IActionResult Index()
        {
            var totalBooks = this.data.Books.Count();


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
            
            return View(new IndexViewModel
            {
                TotalBooks = totalBooks,
                Books = books
            });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
