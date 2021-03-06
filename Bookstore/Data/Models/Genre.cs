namespace Bookstore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreNameLengthMAX)]
        public string Name { get; set; }

        public IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
