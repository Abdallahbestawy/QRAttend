using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.Repositories
{
    public interface ISectionRepo
    {
        Task<int> CreateSection(Section section);
        Task<SectionDto>? GetSectionById(int id);
        Task<List<SectionDto>>? GetAllSections();
        Task<List<SectionDto>>? GetSectionsByAssistantTeacherId(string Id, int? courseId);
        Task<List<SectionDto>>? GetSectionsByGroupId(int? groupId);
    }
}
