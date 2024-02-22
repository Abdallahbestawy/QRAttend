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
        [ForeignKey("User")]
        public string AssistantTeacherId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
        public List<SectionAttendance> SectionAttendances { get; set; }
        public List<AssistantTeacherSection> assistantTeacherSections { get; set; }
        public List<StudentSection> studentSections { get; set; }
    }
}

