using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface ISectionAttendanceRepo
    {
        int Create(SectionAttendance section);
        List<SectionAttendanceDto> GetBySectionId(int id);
        bool IsExsit(SectionAttendance attendance);
        List<StudentsDTO>? GetSudentsBySectionId(int sectionId);
        bool CheckStudentInSection(StudentSectionDTO studentSectionDTO);
        List<StudentLecturesDTO>? GetStudentSections(StudentSectionsByGroupDTO model);
    }
}
