namespace QRAttend.Dto
{
    public class CourseGroupsDTO
    {
        public string CourseName { get; set; }
        public List<SectionGroupDTO>? SectionGroups { get; set; }
        public AddSectionGroupsForUserDTO AddGroupForUser { get; set; }
    }
}
