namespace Bookstore.Models.Books
{
    using Bookstore.Services.Books.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;
    public class BookFormModel : IBookModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Book Title")]
        [StringLength(BookTitleLengthMAX, MinimumLength = BookTitleLengthMIN, ErrorMessage = "Maximum: {0}")]
        public string BookTitle { get; set; }

        [Required]
        [MaxLength(BookAuthorLengthMAX)]
        [MinLength(BookAuthorLengthMIN)]
        public string Author { get; set; }

        [Url]
        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(BookPublishingHouseLengthMAX)]
        [MinLength(BookPublishingHouseLengthMIN)]
        public string PublishingHouse { get; set; }

        [Required]
        public int? Rating { get; set; }

        [Required]
        [MaxLength(BookDescriptionLengthMAX)]
        [MinLength(BookDescriptionLengthMIN)]        
        public string Description { get; set; }

        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        public IEnumerable<BookGenreServiceModel> Genres { get; set; }
    }
}
