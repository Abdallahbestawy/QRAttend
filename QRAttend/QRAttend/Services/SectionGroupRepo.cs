using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;

namespace QRAttend.Services
{
    public class SectionGroupRepo : ISectionGroupRepo
    {
        private readonly QRContext _context;

        public SectionGroupRepo(QRContext context)
        {
            _context = context;
        }

        public bool CheckStudentInGroup(int studentId, int sectionId)
        {
            var group = _context.Sections.FirstOrDefault(s => s.Id == sectionId);
            if (group == null)
                return false;
            return _context.StudentSections.Any(grp => grp.StudentId == studentId && grp.SectionGroupId == group.SectionGroupId);
        }

        public int CreateGroup(SectionGroup sectionGroup)
        {
            _context.SectionGroups.Add(sectionGroup);
            var result = _context.SaveChanges();
            return result;
        }

        public List<SectionGroupDTO>? GetAllSectionGroup()
        {
            var result = _context.SectionGroups.ToList();
            if(result.Count == 0)
                return null;
            return (List<SectionGroupDTO>)result.Select(grp=> new SectionGroupDTO
            {
                Id = grp.Id,
                CourseId = grp.CourseId,
                Name = grp.Name
            }).ToList();
        }

        public SectionGroupDTO? GetSectionGroupById(int id)
        {
            var result = _context.SectionGroups.Select(sec => new SectionGroupDTO
            {
                Id = sec.Id,
                Name = sec.Name,
                CourseId = sec.CourseId
            }).FirstOrDefault(group => group.Id == id);
            if (result == null)
                return null;

            return result;
        }

        public List<SectionGroupDTO>? GetAllSectionGroupByAssistantTeacherId(string id,int courseId)
        {
            var assistantGroups = _context.AssistantTeacherSections.Where(grp=>grp.TeacherId == id).Select(grp => grp.SectionGroupId).ToList();

            var groups = _context.SectionGroups.Where(grp=>assistantGroups.Contains(grp.Id) && grp.CourseId == courseId).ToList();
            if (groups.Count == 0)
                return null;
            return (List<SectionGroupDTO>)groups.Select(grp => new SectionGroupDTO
            {
                Id = grp.Id,
                CourseId = grp.CourseId,
                Name = grp.Name
            }).ToList();
        }
    }
}
