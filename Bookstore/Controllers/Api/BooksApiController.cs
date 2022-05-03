namespace Bookstore.Controllers.Api
{
    using Bookstore.Data;
    using Bookstore.Models;
    using Bookstore.Models.Api.Books;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [ApiController]//защитава и валидира на база на атрибутите
    [Route("api/books")]
    public class BooksApiController : Controller
    {
        private readonly BookstoreDbContext data;

        public BooksApiController(BookstoreDbContext data)
        {
            this.data = data;
        }

        [HttpGet]
        public ActionResult<AllBooksApiResponseModel> All([FromQuery] AllBooksApiRequestModel query)
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
                .Skip((query.CurrentPage - 1) * query.BooksPerPage)
                .Take(query.BooksPerPage)
                .Select(book => new BookResponseModel
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

            return new AllBooksApiResponseModel()
            {
                CurrentPage = query.CurrentPage,
                BooksPerPage = query.BooksPerPage,
                TotalBooks = totalBooks,
                Books = books
            };
        }


        //ModelState се случва автоматично
        //Автоматично изкарва и грешки if(product == null) => NotFound() 

        /*
        [HttpGet]
        public IEnumerable GetCar()
        {
            return this.data.Books.ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDetails(int id)
        {
            var book = this.data.Books.Find(id);

            if(book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        */




    }
}
