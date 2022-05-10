namespace Bookstore.Services.Moderators
{
    using Bookstore.Data;
    using System.Linq;

    public class ModeratorService : IModeratorService
    {
        private readonly BookstoreDbContext data;

        public ModeratorService(BookstoreDbContext data)
        {
            this.data = data;
        }

        public bool IsModerator(string userId)
            => this.data
                .Moderators
                .Any(m => m.UserId == userId);

        
    }
}
