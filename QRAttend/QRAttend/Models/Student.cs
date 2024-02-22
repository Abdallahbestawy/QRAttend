using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAttend.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniversityId { get; set; }
        public string? Token { get; set; }
        public List<Attendance> Attendances { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        public List<StudentSection> studentSections { get; set; }
    }
}
