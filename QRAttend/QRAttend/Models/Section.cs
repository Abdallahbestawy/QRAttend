using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QRAttend.Models
{
    public class Section
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }

        [ForeignKey("sectionGroup")]
        public int SectionGroupId { get; set; }
        public SectionGroup sectionGroup { get; set; }

        [ForeignKey("User")]
        public string AssistantTeacherId { get; set; }
        public ApplicationUser User { get; set; }
        public List<SectionAttendance> SectionAttendances { get; set; }
    }
}

