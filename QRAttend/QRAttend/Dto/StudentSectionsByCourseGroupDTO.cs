using System.ComponentModel.DataAnnotations;

namespace QRAttend.Dto
{
    public class StudentSectionsByGroupDTO
    {
        [Required]
        public string UniversityId { get; set; }
        [Required]
        public int groupId { get; set; }
    }
}
