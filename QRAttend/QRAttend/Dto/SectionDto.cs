using System.ComponentModel.DataAnnotations;

namespace QRAttend.Dto
{
    public class SectionDto
    {
        public int SectionId { get; set; }
        [Required]
        public string Title { get; set; }
        public int SectionGroupId { get; set; }
    }
}
