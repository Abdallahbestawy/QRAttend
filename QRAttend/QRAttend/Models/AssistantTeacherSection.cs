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

        [ForeignKey("sectionGroup")]
        public int SectionGroupId { get; set; }
        public SectionGroup sectionGroup { get; set; }
    }
}
