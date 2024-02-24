using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface ISectionGroupRepo
    {
        int CreateGroup(SectionGroup sectionGroup);
        SectionGroupDTO? GetSectionGroupById(int id);
        List<SectionGroupDTO>? GetAllSectionGroup();
        bool CheckStudentInGroup(int studentId, int sectionId);
        public List<SectionGroupDTO>? GetAllSectionGroupByAssistantTeacherId(string id,int courseId);
    }
}
