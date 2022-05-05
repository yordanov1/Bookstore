namespace Bookstore.Controllers.Api
{
    using Bookstore.Data;
    using Bookstore.Models;
    using Bookstore.Models.Api.Books;
    using Bookstore.Services.Books;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [ApiController]//защитава и валидира на база на атрибутите
    [Route("api/books")]
    public class BooksApiController : Controller
    {
        private readonly IBookService books;

        public BooksApiController(IBookService books)
        {
            this.books = books;
        }


        [HttpGet]
        public BookQueryServiceModel All([FromQuery] AllBooksApiRequestModel query)
        {
            return this.books.All(
                query.Author,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.BooksPerPage);

            /*
            return new AllBooksApiResponseModel()
            {
                CurrentPage = query.CurrentPage,
                BooksPerPage = query.BooksPerPage,
                TotalBooks = totalBooks,
                Books = books
            };
            */
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
