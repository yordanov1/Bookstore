namespace Bookstore.Controllers.Api
{
    using Bookstore.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticServices statistics;

        public StatisticsApiController(IStatisticServices statistics)
        {
            this.statistics = statistics;
        }

        [HttpGet]
        public StatisticsServiceModel GetStatistics()
        {
           return this.statistics.Total();
        }
    }
}
