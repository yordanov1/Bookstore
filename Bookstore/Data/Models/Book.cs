namespace Bookstore.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(BookTitleLengthMAX)]
        public string BookTitle { get; set; }

        [Required]
        [MaxLength(BookAuthorLengthMAX)]
        public string Author { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string PublishingHouse { get; set; }

        public int? Rating { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public string Description { get; set; }

        public int ModeratorId { get; set; }

        public Moderator Moderator { get; set; }

        public bool IsPublic { get; set; }
    }
}
