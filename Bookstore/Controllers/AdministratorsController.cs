namespace Bookstore.Controllers
{
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Bookstore.Infrastructure;
    using Bookstore.Models.Administrators;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class AdministratorsController : Controller
    {
        private readonly BookstoreDbContext data;

        public AdministratorsController(BookstoreDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(BecomeAdministratorFormModel administrator)
        {
            var userId = this.User.GetId();

            var userIdAlreadyAdministrator = this.data
                .Administrators
                .Any(a => a.UserId == userId);

            if (userIdAlreadyAdministrator)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(administrator);
            }


            var administratorData = new Administrator()
            {
                Name = administrator.Name,
                PhoneNumber = administrator.PhoneNumber,
                UserId = userId
            };

            this.data.Add(administratorData);
            this.data.SaveChanges();

            return RedirectToAction("All", "Books");
        }

    }
}
