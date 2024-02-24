using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace QRAttend.Models
{
    public class SectionGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public List<AssistantTeacherSection> assistantTeacherSections { get; set; }
        public List<StudentSection> studentSections { get; set; }
        public List<Section> Sections { get; set; }
    }
}
