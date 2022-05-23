namespace Bookstore.Areas.Admin.Controller
{
    using Bookstore.Services.Books;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : AdminController
    {
        private readonly IBookService books;

        public BooksController(IBookService books)
        {
            this.books = books;
        }

        public IActionResult All()
        {
            var books = this.books.
                All(publicOnly: false).
                Books;

            return View(books);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.books.ChangeVisibility(id);

            return RedirectToAction(nameof(All));
        }

    }
}
