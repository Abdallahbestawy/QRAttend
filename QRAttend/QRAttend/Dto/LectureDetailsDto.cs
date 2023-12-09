namespace QRAttend.Dto
{
    public class LectureDetailsDto
    {
        public string LectureTitle { get; set; }
        public List<String> StudentName { get; set; } = new List<string>();
        public List<String> StudentUniversityId { get; set; } = new List<string>();
    }
}
