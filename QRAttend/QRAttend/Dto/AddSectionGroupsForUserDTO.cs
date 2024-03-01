namespace QRAttend.Dto
{
    public class AddSectionGroupsForUserDTO
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public List<CourseDTO> Courses { get; set; }
        public List<SectionGroupDTO>? SectionGroups { get; set; }
    }
}
