using System.ComponentModel.DataAnnotations;

namespace eduManage.ViewModels
{
    public class LessonsVM
    {
        public int LessonId { get; set; }

        public int ClassId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên bài giảng")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tên mô tả bài giảng")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng số thứ tự hiển thị")]
        public int? OrderIdx { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public bool? IsActive { get; set; }
    }
}
