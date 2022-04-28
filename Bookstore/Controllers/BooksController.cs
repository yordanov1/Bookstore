namespace Bookstore.Controllers
{
    using System;
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
                //_ => booksQuery.OrderByDescending(x => x.Id)
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
                Description= book.Description,
                GenreId = book.GenreId,
            };


            this.data.Books.Add(newBook);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
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
