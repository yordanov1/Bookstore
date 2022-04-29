namespace Bookstore.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Administrator
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(AdministratorNameLengthMAX)]
        public string Name { get; set; }

        [Required]
        [MaxLength(AdministratorPhoneLengthMAX)]
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }

        public IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
