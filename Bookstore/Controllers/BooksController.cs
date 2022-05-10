namespace Bookstore.Controllers
{
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Bookstore.Infrastructure;
    using Bookstore.Models;
    using Bookstore.Models.Books;
    using Bookstore.Services.Books;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class BooksController : Controller
    {
        private readonly BookstoreDbContext data;
        private readonly IBookService books;

        public BooksController(BookstoreDbContext data, IBookService books)
        {
            this.data = data;   
            this.books = books;
        }



        //public IActionResult All([FromQuery] AllBooksQueryModel query) { } - класовете не се байндват  от GET заявка затова слагаме [FromQuery]
        //public IActionResult All(string author, string searchTerm, BookSorting sorting)
        public IActionResult All([FromQuery] AllBooksQueryModel query)
        {
            var queryResult = this.books.All(
                query.Author,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllBooksQueryModel.BooksPerPage);

            var bookAuthors = this.books.AllBookAuthors();

            query.Authors = bookAuthors;
            query.TotalBooks = queryResult.TotalBooks;
            query.Books = queryResult.Books;
           
            return View(query);
        }



        [Authorize]
        public IActionResult Add()
        {

             if (!this.UserIsModerator())
             {                
                 return RedirectToAction(nameof(ModeratorsController.Create), "Moderators");
             }


             return View(new AddBookFormModel
             {
                 Genres = this.GetBookGenres()
             });
        } 


        [HttpPost]
        [Authorize]
        public IActionResult Add(AddBookFormModel book)
        {
            var moderatorId = this.data
                .Moderators
                .Where(a => a.UserId == this.User.GetId())
                .Select(a => a.Id)
                .FirstOrDefault();


            if (moderatorId == 0)
            {
                return RedirectToAction(nameof(ModeratorsController.Create), "Moderators");
            }


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
                Description= book.Description,
                GenreId = book.GenreId,
                ModeratorId = moderatorId,
            };

            this.data.Books.Add(newBook);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }


        public IActionResult Delete(int id)
        {
            var bookDelete = this.data.Books.FirstOrDefault(x => x.Id == id);

            this.data.Books.Remove(bookDelete);
            this.data.SaveChanges();

            return RedirectToAction("All");
        }



        private bool UserIsModerator()
            => this.data
            .Moderators
            .Any(a => a.UserId == this.User.GetId());


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
