using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface ISectionAttendanceRepo
    {
        Task<int> Create(SectionAttendance attendance);
        Task<List<SectionAttendanceDto>> GetBySectionId(int id);
        Task<bool> IsExsit(SectionAttendance attendance);
        Task<List<StudentsDTO>>? GetSudentsBySectionId(int sectionId);
        Task<bool> CheckStudentInSection(StudentSectionDTO studentSectionDTO);
        Task<List<StudentLecturesDTO>>? GetStudentSections(StudentSectionsByGroupDTO model);
    }
}
