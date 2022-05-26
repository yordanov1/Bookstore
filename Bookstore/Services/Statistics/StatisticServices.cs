namespace Bookstore.Services.Statistics
{
    using Bookstore.Data;
    using System.Linq;

    public class StatisticServices : IStatisticServices
    {
        private readonly BookstoreDbContext data;

        public StatisticServices(BookstoreDbContext data)
        {
            this.data = data;
        }

        public StatisticsServiceModel Total()
        {
            var totalBooks = this.data.Books.Count(x => x.IsPublic);
            var totalUsers = this.data.Users.Count();
            var totalModerators = this.data.Moderators.Count();

            return new StatisticsServiceModel
            {
                TotalBooks = totalBooks,
                TotalUsers = totalUsers,
                TotalModerators = totalModerators
            };
        }
    }
}
