﻿using Microsoft.EntityFrameworkCore;
using QRAttend.Dto;
using QRAttend.Models;
using QRAttend.Repositories;
using System.Linq;

namespace QRAttend.Services
{
    public class SectionGroupRepo : ISectionGroupRepo
    {
        private readonly QRContext _context;

        public SectionGroupRepo(QRContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckStudentInGroup(int studentId, int sectionId)
        {
            var group = await _context.Sections.FirstOrDefaultAsync(s => s.Id == sectionId);
            if (group == null)
                return false;
            return await _context.StudentSections.AnyAsync(grp => grp.StudentId == studentId && grp.SectionGroupId == group.SectionGroupId);
        }

        public async Task<int> CreateGroup(SectionGroup sectionGroup)
        {
            await _context.SectionGroups.AddAsync(sectionGroup);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<SectionGroupDTO>>? GetAllSectionGroup()
        {
            var result = await _context.SectionGroups.ToListAsync();
            if(result.Count == 0)
                return null;
            return result.Select(grp => new SectionGroupDTO
            {
                Id = grp.Id,
                CourseId = grp.CourseId,
                Name = grp.Name
            }).ToList();
        }

        public async Task<SectionGroupDTO>? GetSectionGroupById(int id)
        {
            var result = await _context.SectionGroups.Select(sec => new SectionGroupDTO
            {
                Id = sec.Id,
                Name = sec.Name,
                CourseId = sec.CourseId
            }).FirstOrDefaultAsync(group => group.Id == id);
            if (result == null)
                return null;

            return result;
        }

        public async Task<List<SectionGroupDTO>>? GetAllSectionGroupByAssistantTeacherId(string id,int courseId)
        {
            var assistantGroups = await _context.AssistantTeacherSections.Where(grp=>grp.TeacherId == id).Select(grp => grp.SectionGroupId).ToListAsync();

            var groups = await _context.SectionGroups.Where(grp=>assistantGroups.Contains(grp.Id) && grp.CourseId == courseId).ToListAsync();
            if (groups.Count == 0)
                return null;
            return groups.Select(grp => new SectionGroupDTO
            {
                Id = grp.Id,
                CourseId = grp.CourseId,
                Name = grp.Name
            }).ToList();
        }

        public async Task<List<SectionGroupDTO>>? GetAllSectionGroupByCourseId(int courseId)
        {
            var groups = await _context.SectionGroups.Where(sec=>sec.CourseId == courseId).ToListAsync();

            var result = groups.Select(sec => new SectionGroupDTO
            {
                Id = sec.Id,
                CourseId = sec.CourseId,
                Name = sec.Name
            }).ToList();

            return result;
        }

        public async Task<bool> AddUserToSectionGroups(AddSectionGroupsForUserDTO model)
        {
            if (model == null || model.SectionGroups == null)
            {
                // Handle invalid input
                return false;
            }

            var usergroups = await _context.AssistantTeacherSections
                .Where(grp => grp.TeacherId == model.UserId && grp.sectionGroup.CourseId == model.CourseId)
                .ToListAsync();

            var newgroups = model.SectionGroups;

            if (usergroups == null)
            {
                foreach (var newGroup in newgroups)
                {
                    if (newGroup.IsSelected)
                    {
                        _context.AssistantTeacherSections.Add(new AssistantTeacherSection
                        {
                            TeacherId = model.UserId,
                            SectionGroupId = newGroup.Id
                        });
                    }
                }
            }
            else
            {
                foreach (var newGroup in newgroups)
                {
                    if (newGroup.IsSelected)
                    {
                        if (!usergroups.Any(ug => ug.SectionGroupId == newGroup.Id))
                        {
                            _context.AssistantTeacherSections.Add(new AssistantTeacherSection
                            {
                                TeacherId = model.UserId,
                                SectionGroupId = newGroup.Id
                            });
                        }
                    }
                    else
                    {
                        var existingGroup = usergroups.FirstOrDefault(ug => ug.SectionGroupId == newGroup.Id);
                        if (existingGroup != null)
                        {
                            _context.AssistantTeacherSections.Remove(existingGroup);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
