namespace Bookstore.Models.Moderators
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class BecomeModeratorFormModel
    {
        [Required]
        [StringLength(ModeratorNameLengthMAX, MinimumLength = ModeratorNameLengthMIN)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(ModeratorPhoneLengthMAX, MinimumLength = ModeratorPhoneLengthMIN)]
        public string PhoneNumber { get; set; }
    }
}
