namespace Bookstore.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class User : IdentityUser
    {
        [MaxLength(UserNameLengthMAX)]
        public string FullName { get; set; }
    }
}
