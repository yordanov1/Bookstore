namespace Bookstore.Controllers.Api
{
    using Bookstore.Data;
    using Bookstore.Models.Api.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly BookstoreDbContext data;

        public StatisticsApiController(BookstoreDbContext data)
        {
            this.data = data;
        }

        [HttpGet]
        public StatisticsResponceModel GetStatistics()
        {
            var totalBooks = this.data.Books.Count();
            var TotalUsers = this.data.Users.Count();

            return new StatisticsResponceModel()
            {
                TotalBooks = totalBooks,
                TotalUsers = TotalUsers,
                TotalRents = 0
            };
        }




    }

}
