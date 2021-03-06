namespace Bookstore.Controllers
{
    using AutoMapper;
    using Bookstore.Infrastructure.Extensions;
    using Bookstore.Models.Books;
    using Bookstore.Services.Books;
    using Bookstore.Services.Moderators;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : Controller
    {
        private readonly IBookService books;
        private readonly IModeratorService moderators;
        private readonly IMapper mapper;

        public BooksController(
            IBookService books,
            IModeratorService moderators, 
            IMapper mapper)
        {
            this.books = books;
            this.moderators = moderators;
            this.mapper = mapper;
        }


        //-public IActionResult All([FromQuery] AllBooksQueryModel query) { } 
        //-Classes are not bound by GET requests so we put [FromQuery]
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
        public IActionResult Mine()
        {
            var myBooks = this.books.ByUser(this.User.Id());

            return View(myBooks);
        }


        public IActionResult Information(int id, string information)
        {            
            var bookInfo = this.books.GetBookInfo(id);

            if (information != bookInfo.GetInformation())
            {
                return BadRequest();
            }

            return View(bookInfo);
        }


        [Authorize]
        public IActionResult Add()
        {

             if (!this.moderators.IsModerator(this.User.Id()))
             {                
                 return RedirectToAction(nameof(ModeratorsController.Create), "Moderators");
             }


             return View(new BookFormModel
             {
                 Genres = this.books.AllBookGenres()
             });
        } 


        [HttpPost]
        [Authorize]
        public IActionResult Add(BookFormModel book)
        {
            var moderatorId = this.moderators.IdByUser(this.User.Id());


            if (moderatorId == 0)
            {
                return RedirectToAction(nameof(ModeratorsController.Create), "Moderators");
            }


            if (!this.books.GenreExist(book.GenreId))
            {
                this.ModelState.AddModelError(nameof(book.GenreId), "Genre does not exist!");
            }


            //ModelState complies with the attributes we have written in DTO
            if (!ModelState.IsValid)
            {
                book.Genres = this.books.AllBookGenres();

                return View(book);
            }

            var bookId = books.Create(
                book.BookTitle,
                book.Author,
                book.ImageUrl,
                book.PublishingHouse,
                book.Rating,
                book.Description,
                book.GenreId,
                moderatorId);

            return RedirectToAction(nameof(Information), new { id = bookId, information = book.GetInformation()});
        }


        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.Id();


            if (!this.moderators.IsModerator(userId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.books.DeleteBook(id);

            return RedirectToAction("All");
        }
        

        [Authorize]
        public IActionResult Edit(int id)
         {
            var userId = this.User.Id();

            if (!this.moderators.IsModerator(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(ModeratorsController.Create), "Moderators");
            }

            var book = this.books.Details(id);

            if (book.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }


            var bookForm = this.mapper.Map<BookFormModel>(book);
            bookForm.Genres = this.books.AllBookGenres();

            return View(bookForm);

            /*
            return View(new BookFormModel
            {
                BookTitle = book.BookTitle,
                Author = book.Author,
                ImageUrl = book.ImageUrl,
                PublishingHouse = book.PublishingHouse,
                Rating = book.Rating,
                Description = book.Description,
                GenreId = book.GenreId,
                Genres = this.books.AllBookGenres()
            });
            */
        }


        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, BookFormModel book)
        {
            var moderatorId = this.moderators.IdByUser(this.User.Id());


            if (moderatorId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(ModeratorsController.Create), "Moderators");
            }

            if (!this.books.GenreExist(book.GenreId))
            {
                this.ModelState.AddModelError(nameof(book.GenreId), "Genre does not exist!");
            }
            
            if (!ModelState.IsValid)
            {
                book.Genres = this.books.AllBookGenres();

                return View(book);
            }

            if (!this.books.IsByModerator(id, moderatorId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            var edited = this.books.Edit(
                id,
                book.BookTitle,
                book.Author,
                book.ImageUrl,
                book.PublishingHouse,
                book.Rating,
                book.Description,
                book.GenreId,
                this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Information), new { id = id, information = book.GetInformation() });
        }
    }
}


