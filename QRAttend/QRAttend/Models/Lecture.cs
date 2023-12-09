using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAttend.Models
{
    public class Lecture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        [ForeignKey("User")]
        public string DoctorId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Attendance> Attendances { get; set; }
    }
}
