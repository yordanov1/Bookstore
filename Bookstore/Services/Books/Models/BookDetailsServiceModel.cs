namespace Bookstore.Services.Books.Models
{
    public class BookDetailsServiceModel : BookServiceModel
    {
        public string Description { get; set; }

        public string GenreName { get; set; }

        public int ModeratorId { get; set; }

        public string ModeratorName { get; set; }

        public int GenreId { get; set; }

        public string UserId { get; set; }
    }
}
