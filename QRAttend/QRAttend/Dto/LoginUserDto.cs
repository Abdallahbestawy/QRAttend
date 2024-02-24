using System.ComponentModel.DataAnnotations;

namespace QRAttend.Dto
{
    public class LoginUserDto
    {
        [Required]
      public string UserName { get; set; }
        [Required]
      public string Password { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
