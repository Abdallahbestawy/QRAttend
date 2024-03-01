using System.ComponentModel.DataAnnotations;

namespace QRAttend.Dto
{
    public class AddSectionGroupDTO
    {
        [Required]
        [StringLength(256)]
        [Display(Name = "Group Name")]
        public string Name { get; set; }
        public int CourseId { get; set; }
    }
}
