using System.ComponentModel.DataAnnotations;

namespace QRAttend.Dto
{
    public class SectionAttendanceDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string UniversityStudentId { get; set; }
        [Required]
        public int SectionId { get; set; }
    }
}
