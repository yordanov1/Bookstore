namespace Bookstore.Controllers
{
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Bookstore.Infrastructure;
    using Bookstore.Models;
    using Bookstore.Models.Books;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class BooksController : Controller
    {
        private readonly BookstoreDbContext data;

        public BooksController(BookstoreDbContext data)
            => this.data = data;



        //public IActionResult All([FromQuery] AllBooksQueryModel query) { } - класовете не се байндват  от GET заявка затова слагаме [FromQuery]
        //public IActionResult All(string author, string searchTerm, BookSorting sorting)
        public IActionResult All([FromQuery] AllBooksQueryModel query)
        {
            var booksQuery = this.data.Books.AsQueryable();


            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                booksQuery = booksQuery.Where(x => x.Author == query.Author);
            }


            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                booksQuery = booksQuery.Where(x =>
                       (x.BookTitle + " " + x.Author).ToLower().Contains(query.SearchTerm.ToLower())
                     || x.Description.ToLower().Contains(query.SearchTerm.ToLower()));                    
            }


            booksQuery = query.Sorting switch
            {
                BookSorting.Rating => booksQuery.OrderByDescending(x => x.Rating),
                BookSorting.Author => booksQuery.OrderByDescending(x => x.Author),
                _ => booksQuery.OrderByDescending(x => x.Id)
            };


            var totalBooks = booksQuery.Count();


            var books = booksQuery     
                .Skip((query.CurrentPage - 1) * AllBooksQueryModel.BooksPerPage)
                .Take(AllBooksQueryModel.BooksPerPage)
                .Select(book => new BookListingViewModel
                {
                    Id = book.Id,
                    BookTitle = book.BookTitle,
                    Author = book.Author,
                    ImageUrl = book.ImageUrl,
                    PublishingHouse = book.PublishingHouse,
                    Rating = book.Rating,
                    Description = book.Description,
                    Genre = book.Genre.Name                    
                })
                .ToList();

            var bookAuthors = this.data
                .Books
                .Select(x => x.Author)
                .Distinct()
                .ToList();

            query.TotalBooks = totalBooks;
            query.Authors = bookAuthors;
            query.Books = books;

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {

             if (!this.UserIsAdministrator())
             {                
                 return RedirectToAction(nameof(AdministratorsController.Create), "Administrators");
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
            var administratorId = this.data
                .Administrators
                .Where(a => a.UserId == this.User.GetId())
                .Select(a => a.Id)
                .FirstOrDefault();


            if (administratorId == 0)
            {
                return RedirectToAction(nameof(AdministratorsController.Create), "Administrators");
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
                AdministratorId = administratorId,
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



        private bool UserIsAdministrator()
            => this.data
            .Administrators
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
