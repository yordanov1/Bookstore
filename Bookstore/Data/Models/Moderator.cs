namespace Bookstore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Moderator
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(ModeratorNameLengthMAX)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ModeratorPhoneLengthMAX)]
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }

        public IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
