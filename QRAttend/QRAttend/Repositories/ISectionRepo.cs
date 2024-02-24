using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface ISectionRepo
    {
        int CreateSection(Section section);
        SectionDto? GetSectionById(int id);
        List<SectionDto>? GetAllSections();
        List<SectionDto>? GetSectionsByAssistantTeacherId(string Id,int? courseId);
        List<SectionDto>? GetSectionsByGroupId(int? groupId);
    }
}
