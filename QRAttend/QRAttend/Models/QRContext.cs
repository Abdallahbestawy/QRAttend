using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QRAttend.Models
{
    public class QRContext:IdentityDbContext<ApplicationUser>
    {
        public QRContext(DbContextOptions<QRContext> options):base(options)
        {
        }
        public DbSet<Lecture> Lectures  { get; set; }
        public DbSet<Student> Students  { get; set; }
        public DbSet<Attendance> Attendances  { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<SectionAttendance> SectionAttendances { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<AssistantTeacherSection> AssistantTeacherSections { get; set; }
        public DbSet<StudentSection> StudentSections { get; set; }
        public DbSet<SectionGroup> SectionGroups { get; set; }
    }
}
