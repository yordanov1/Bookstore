namespace Bookstore.Models.Administrators
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class BecomeAdministratorFormModel
    {
        [Required]
        [StringLength(AdministratorNameLengthMAX, MinimumLength = AdministratorNameLengthMIN)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(AdministratorPhoneLengthMAX, MinimumLength = AdministratorPhoneLengthMIN)]
        public string PhoneNumber { get; set; }
    }
}
