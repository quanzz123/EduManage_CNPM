using System.ComponentModel.DataAnnotations;
namespace eduManage.ViewModels
{
    public class AssignmentVM
    {
        public int AssignmentId { get; set; }

        public int ClassId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? FileUrl { get; set; }

        public DateTime? Deadline { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public bool? IsActive { get; set; }

        [Display(Name = "Tải file lên")]
        public IFormFile? FileUpload { get; set; } // file người dùng upload

    }
}
