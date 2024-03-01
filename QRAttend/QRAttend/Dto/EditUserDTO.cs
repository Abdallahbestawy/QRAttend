using System.ComponentModel.DataAnnotations;

namespace QRAttend.Dto
{
    public class EditUserDTO
    {
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
