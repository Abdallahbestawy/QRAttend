using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAttend.Models
{
    public class AssistantTeacherSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string TeacherId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
