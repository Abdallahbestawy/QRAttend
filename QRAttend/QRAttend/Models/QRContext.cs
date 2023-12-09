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
    }
}
