using QRAttend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAttend.Dto
{
    public class LectureDto
    {
        public int? LectureId { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
