using Microsoft.EntityFrameworkCore;
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

        public async Task<int> CreateSection(Section section)
        {
            await _context.Sections.AddAsync(section);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<SectionDto>>? GetAllSections()
        {
            var result = await _context.Sections.ToListAsync();
            if(result ==  null)
                return null;
            return result.Select(sec => new SectionDto
            {
                SectionId = sec.Id,
                Title = sec.Title,
                SectionGroupId = sec.SectionGroupId
            }).ToList();
        }

        public async Task<SectionDto>? GetSectionById(int id)
        {
            var result = await _context.Sections.FirstOrDefaultAsync(Section => Section.Id == id);
            if(result == null)
                return null;
            return new SectionDto
            {
                SectionId = result.Id,
                Title = result.Title,
                SectionGroupId = result.SectionGroupId
            };
        }

        public async Task<List<SectionDto>>? GetSectionsByAssistantTeacherId(string Id, int? courseId)
        {
            var courseGroupIds = await _context.SectionGroups
                .Where(grp => grp.CourseId == courseId)
                .Select(grp => grp.Id)
                .ToListAsync();

            if (courseGroupIds.Count == 0)
                return null;

            var assistantGroups = await _context.AssistantTeacherSections
                .Where(assist => assist.TeacherId == Id && courseGroupIds.Contains(assist.SectionGroupId))
                .ToListAsync();

            if (assistantGroups.Count == 0)
                return null;
            var sectionGroupIds = assistantGroups.Select(ag => ag.SectionGroupId).ToList();

            var result = await _context.Sections
                .Where(sec => sectionGroupIds.Contains(sec.SectionGroupId))
                .Select(sec => new SectionDto
                {
                    SectionId = sec.Id,
                    Title = sec.Title,
                    SectionGroupId = sec.SectionGroupId
                }).ToListAsync();

            if (result.Count == 0)
                return null;

            return result;
        }

        public async Task<List<SectionDto>>? GetSectionsByGroupId(int? groupId)
        {
            return await _context.Sections.Where(grp=>grp.SectionGroupId == groupId).Select(group=> new SectionDto
            {
                SectionId = group.Id,
                Title = group.Title,
                SectionGroupId = group.SectionGroupId
            }).ToListAsync();
        }
    }
}
