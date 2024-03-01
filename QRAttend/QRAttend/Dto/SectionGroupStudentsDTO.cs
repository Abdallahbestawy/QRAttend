namespace QRAttend.Dto
{
    public class SectionGroupStudentsDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<NewStudentDTO> Students { get; set; }
    }
}
