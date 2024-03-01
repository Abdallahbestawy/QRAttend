using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface ISectionGroupRepo
    {
        Task<int> CreateGroup(SectionGroup sectionGroup);
        Task<SectionGroupDTO>? GetSectionGroupById(int id);
        Task<List<SectionGroupDTO>>? GetAllSectionGroup();
        Task<List<SectionGroupDTO>>? GetAllSectionGroupByCourseId(int courseId);
        Task<bool> CheckStudentInGroup(int studentId, int sectionId);
        Task<List<SectionGroupDTO>>? GetAllSectionGroupByAssistantTeacherId(string id, int courseId);
        Task<bool> AddUserToSectionGroups(AddSectionGroupsForUserDTO model);
    }
}
