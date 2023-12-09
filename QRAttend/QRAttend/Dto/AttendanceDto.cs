using QRAttend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAttend.Dto
{
    public class AttendanceDto
    {
        public string MacAddressStudent { get; set; }
        public int StudentId { get; set; }
       
        public int LectureId { get; set; }
       
    }
}
