using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAttend.Models
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string MacAddressStudent { get; set; }
        public DateTime? CurrentDate { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [ForeignKey("Lecture")]
        public int LectureId { get; set; }
        public Lecture Lecture { get; set; }
    }
}
