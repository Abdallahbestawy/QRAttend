using System.ComponentModel.DataAnnotations;

namespace QRAttend.Dto
{
    public class AddCourseDTO
    {
        [Required]
        [StringLength(256)]
        [Display(Name = "Course Name")]
        public string Name { get; set; }
    }
}
