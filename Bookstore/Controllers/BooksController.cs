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

        public IActionResult All(
            string author,
            string searchTerm,
            BookSorting sorting)
        {
            var booksQuery = this.data.Books.AsQueryable();


            if (!string.IsNullOrWhiteSpace(author))
            {
                booksQuery = booksQuery.Where(x => x.Author == author);
            }


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                booksQuery = booksQuery.Where(x =>
                       (x.BookTitle + " " + x.Author).ToLower().Contains(searchTerm.ToLower())
                     || x.Description.ToLower().Contains(searchTerm.ToLower()));                    
            }


            booksQuery = sorting switch
            {
                BookSorting.Rating => booksQuery.OrderByDescending(x => x.Id),
                BookSorting.Author => booksQuery.OrderByDescending(x => x.Author),
                //_ => booksQuery.OrderByDescending(x => x.Id)
            };


            var books = booksQuery
                .OrderByDescending(x => x.Id)
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

            return View(new AllBooksQueryModel
            {
                Author = author,
                Authors = bookAuthors,
                Books = books,
                SearchTerm = searchTerm,
                Sorting = sorting
                
            });
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
