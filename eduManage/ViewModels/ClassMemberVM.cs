using System.ComponentModel.DataAnnotations;

namespace eduManage.ViewModels
{
    public class ClassMemberVM
    {
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học viên.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày tham gia.")]
        public DateTime JoinDate { get; set; }

        public string? Status { get; set; }
        public int? Progress { get; set; }
        public double? FinalScore { get; set; }
        public string? Note { get; set; }
    }
}
