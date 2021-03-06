namespace Bookstore.Services.Books.Models
{
    public class BookServiceModel : IBookModel
    {
        public int Id { get; set; }

        public string BookTitle { get; set; }

        public string Author { get; set; }

        public string ImageUrl { get; set; }

        public string PublishingHouse { get; set; }

        public int? Rating { get; set; }

        public string Description { get; set; }

        public string GenreName { get; set; }

        public bool IsPublic { get; set; }
    }
}



