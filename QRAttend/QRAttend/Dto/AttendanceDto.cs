﻿using QRAttend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAttend.Dto
{
    public class AttendanceDto
    {
        public string token { get; set; }
        public string UniversityStudentId { get; set; }
       
        public int LectureId { get; set; }
       
    }
}
