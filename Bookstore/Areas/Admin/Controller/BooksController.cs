namespace Bookstore.Areas.Admin.Controller
{
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : AdminController
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
