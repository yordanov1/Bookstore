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


        //public decimal? Price { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        //public string Isbn { get; set; }


        //public DateTime DatePublished { get; set; }


        //public string Language { get; set; }


        //public int Pages { get; set; }


        //public int Reviews { get; set; } //broj ocenki

        //// Paperback, Hardback

        //public string Cover { get; set; }

        //public string Comments { get; set; }


        public string Description { get; set; }
    }
}
