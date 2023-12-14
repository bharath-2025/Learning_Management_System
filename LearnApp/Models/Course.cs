namespace LearnApp.Models
{
    public class Course{
        public string? CourseId { get; set; }
        public string? CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public string? BatchId { get; set; }

        public byte[]? VideoPath{ get;set; }
        public byte[]? PhotoPath{ get;set; }

    }

}