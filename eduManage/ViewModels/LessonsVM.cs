namespace eduManage.ViewModels
{
    public class LessonsVM
    {
        public int LessonId { get; set; }

        public int ClassId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int? OrderIdx { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public bool? IsActive { get; set; }
    }
}
