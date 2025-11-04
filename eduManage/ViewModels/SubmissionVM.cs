using System.ComponentModel.DataAnnotations;

namespace eduManage.ViewModels
{
    public class SubmissionVM
    {
        public int SubmissionId { get; set; }

        [Required(ErrorMessage = "Thiếu mã bài tập")]
        public int AssignmentId { get; set; }

        [Required(ErrorMessage = "Thiếu mã học sinh")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Vui lòng tải lên file bài nộp")]
        public string? FileUrl { get; set; }

        public DateTime? SubmitDate { get; set; }

        public double? Score { get; set; }

        public string? Feedback { get; set; }

        public string? Status { get; set; }

        public string? StudientName { get; set; }
    }
}
