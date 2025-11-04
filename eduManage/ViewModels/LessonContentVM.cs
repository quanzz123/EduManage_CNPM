using System.ComponentModel.DataAnnotations;

namespace eduManage.ViewModels
{
    public class LessonContentVM
    {
        public int ContentId { get; set; }

        public int LessionId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên bài giảng")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn loại bài giảng")]
        public string? ContentType { get; set; }

        public string? ContentUrl { get; set; }

            
        [Required(ErrorMessage = "Vui lòng nhạp thời lượng")]
        public int? Duration { get; set; }

        public int? OrderIdx { get; set; }

        public DateTime? CreateDate { get; set; }

        [Display(Name = "Tải file lên")]
        public IFormFile? FileUpload { get; set; }
    }
}
