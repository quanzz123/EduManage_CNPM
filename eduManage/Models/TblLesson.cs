using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eduManage.Models
{
    [Table("tblLessons")]
    public partial class TblLesson
    {
        [Key]
        public int LessonId { get; set; }

        public int ClassId { get; set; }

        [StringLength(250)]
        public string Title { get; set; } = null!;

        [StringLength(250)]
        public string? Description { get; set; }

        public int? OrderIdx { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public bool? IsActive { get; set; }

        [ForeignKey("ClassId")]
        [InverseProperty("TblLessons")]
        public virtual TblClass Class { get; set; } = null!;

        [InverseProperty("Lesson")]
        public virtual ICollection<TblLearningProgress> TblLearningProgresses { get; set; } = new List<TblLearningProgress>();

        [InverseProperty("Lession")]
        public virtual ICollection<TblLessionContent> TblLessionContents { get; set; } = new List<TblLessionContent>();
    }
}
