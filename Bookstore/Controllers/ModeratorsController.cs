namespace Bookstore.Controllers
{
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Bookstore.Infrastructure.Extensions;
    using Bookstore.Models.Moderators;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class ModeratorsController : Controller
    {
        private readonly BookstoreDbContext data;

        public ModeratorsController(BookstoreDbContext data)
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
        public IActionResult Create(BecomeModeratorFormModel moderator)
        {
            var userId = this.User.Id();

            var userIdAlreadyModerator = this.data
                .Moderators
                .Any(a => a.UserId == userId);

            if (userIdAlreadyModerator)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(moderator);
            }


            var moderatorData = new Moderator()
            {
                Name = moderator.Name,
                PhoneNumber = moderator.PhoneNumber,
                UserId = userId
            };

            this.data.Add(moderatorData);
            this.data.SaveChanges();

            return RedirectToAction("All", "Books");
        }

    }
}
