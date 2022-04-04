namespace Bookstore.Controllers
{
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Bookstore.Models.Books;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class BooksController : Controller
    {
        private readonly BookstoreDbContext data;

        public BooksController(BookstoreDbContext data)
            => this.data = data;

        public IActionResult Add() => View(new AddBookFormModel
        {
            Genres = this.GetBookGenres()
        });


        [HttpPost]
        public IActionResult Add(AddBookFormModel book)
        {

            if (!this.data.Genres.Any(x => x.Id == book.GenreId))
            {
                this.ModelState.AddModelError(nameof(book.GenreId), "Genre does not exist!");
            }



            //ModelState се съобразява с атрибутите които сме написали в ДТО-то
            if (!ModelState.IsValid)
            {
                book.Genres = this.GetBookGenres();
                return View(book);
            }


            var newBook = new Book
            {
                BookTitle = book.BookTitle,
                Author = book.Author,
                ImageUrl = book.ImageUrl,
                PublishingHouse = book.PublishingHouse,
                Rating = book.Rating,
                Description = book.Description,
                GenreId = book.GenreId,
            };

            this.data.Books.Add(newBook);
            this.data.SaveChanges();





            return RedirectToAction ("Index", "Home");
        }



        // HTTP рекуеста който идва от формата в браузъра да се намачне на AddBookFormModel book
        private IEnumerable<BookGenreViewModel> GetBookGenres()
             => this.data
             .Genres
             .Select(x => new BookGenreViewModel
             {
                 Id = x.Id,
                 Name = x.Name
             })
             .ToList();
    }
}
