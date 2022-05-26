namespace Bookstore.Services.Books.Models
{
    using Bookstore.Data.Models;

    public class BookInformationServiceModel : IBookModel
    {
        public int Id { get; set; }

        public string BookTitle { get; set; }
       
        public string Author { get; set; }
       
        public string ImageUrl { get; set; }
        
        public string PublishingHouse { get; set; }

        public int? Rating { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public string Description { get; set; }
    }
}
