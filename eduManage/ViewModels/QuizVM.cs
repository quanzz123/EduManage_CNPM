using System.ComponentModel.DataAnnotations;

namespace eduManage.ViewModels
{
    public class QuizVM
    {
        public int QuizId { get; set; }

        [Required(ErrorMessage = "cần phải có id lớp học")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        public string Title { get; set; } = null!;

        public string? Descriptions { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thời gian")]
        public int? Duration { get; set; }

        public DateTime? CreateTime { get; set; }

        public bool? Isactive { get; set; }
    }
}
