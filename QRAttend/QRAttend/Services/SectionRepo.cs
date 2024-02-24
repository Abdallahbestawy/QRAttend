using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Services
{
    public class SectionRepo : ISectionRepo
    {
        private readonly QRContext _context;

        public SectionRepo(QRContext context)
        {
            _context = context;
        }

        public int CreateSection(Section section)
        {
            _context.Sections.Add(section);
            var result = _context.SaveChanges();
            return result;
        }

        public List<SectionDto>? GetAllSections()
        {
            var result = _context.Sections.ToList();
            if(result ==  null)
                return null;
            return (List<SectionDto>)result.Select(sec => new SectionDto
            {
                SectionId = sec.Id,
                Title = sec.Title,
                SectionGroupId = sec.SectionGroupId
            }).ToList();
        }

        public SectionDto? GetSectionById(int id)
        {
            var result = _context.Sections.FirstOrDefault(Section => Section.Id == id);
            if(result == null)
                return null;
            return new SectionDto
            {
                SectionId = result.Id,
                Title = result.Title,
                SectionGroupId = result.SectionGroupId
            };
        }

        public List<SectionDto>? GetSectionsByAssistantTeacherId(string Id, int? courseId)
        {
            var courseGroupIds = _context.SectionGroups
                .Where(grp => grp.CourseId == courseId)
                .Select(grp => grp.Id)
                .ToList();

            if (courseGroupIds.Count == 0)
                return null;

            var assistantGroups = _context.AssistantTeacherSections
                .Where(assist => assist.TeacherId == Id && courseGroupIds.Contains(assist.SectionGroupId))
                .ToList();

            if (assistantGroups.Count == 0)
                return null;
            var sectionGroupIds = assistantGroups.Select(ag => ag.SectionGroupId).ToList();

            var result = _context.Sections
                .Where(sec => sectionGroupIds.Contains(sec.SectionGroupId))
                .Select(sec => new SectionDto
                {
                    SectionId = sec.Id,
                    Title = sec.Title,
                    SectionGroupId = sec.SectionGroupId
                })
                .ToList();

            if (result.Count == 0)
                return null;

            return result;
        }

        public List<SectionDto>? GetSectionsByGroupId(int? groupId)
        {
            return _context.Sections.Where(grp=>grp.SectionGroupId == groupId).Select(group=> new SectionDto
            {
                SectionId = group.Id,
                Title = group.Title,
                SectionGroupId = group.SectionGroupId
            }).ToList();
        }
    }
}
