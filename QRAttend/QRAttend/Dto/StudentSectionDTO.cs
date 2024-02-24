using System.ComponentModel.DataAnnotations;

namespace QRAttend.Dto
{
    public class StudentSectionDTO
    {
        [Required]
        public string UniversityId { get; set; }
        [Required]
        public int SectionId { get; set; }
    }
}
