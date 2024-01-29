using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;

namespace QRAttend.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("AcademicYear")]
        public int AcademicYearId { get; set; }
        [ForeignKey("User")]
        public string TeacherId { get; set; }
        public ApplicationUser User { get; set; }
        public AcademicYear AcademicYear { get; set; }
        public List<Lecture> Lectures { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
    }
}
