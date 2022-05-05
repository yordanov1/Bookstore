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
            var totalBooks = this.data.Books.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalBooks = totalBooks,
                TotalUsers = totalUsers,                
            };
        }
    }
}
