using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eduManage.ViewModels
{
    public class ClassesVM
    {
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        public string ClassName { get; set; } = null!;

        public string? Description { get; set; }

        [StringLength(250)]
        public string? Subject { get; set; }

        [Required(ErrorMessage ="TeacherId is required")]
        public int? TeacherId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string? Schedule { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(250)]
        public string? Image { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifedDate { get; set; }

        public int? MaxStudents { get; set; }
    }
}
