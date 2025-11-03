using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblLesson
{
    public int LessonId { get; set; }

    public int ClassId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? OrderIdx { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? CreateBy { get; set; }

    public bool? IsActive { get; set; }

    public virtual TblClass Class { get; set; } = null!;

    public virtual ICollection<TblLearningProgress> TblLearningProgresses { get; set; } = new List<TblLearningProgress>();

    public virtual ICollection<TblLessionContent> TblLessionContents { get; set; } = new List<TblLessionContent>();
}
