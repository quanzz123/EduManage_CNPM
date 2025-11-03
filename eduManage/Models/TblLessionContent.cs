using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblLessionContent
{
    public int ContentId { get; set; }

    public int LessionId { get; set; }

    public string Title { get; set; } = null!;

    public string? ContentType { get; set; }

    public string? ContentUrl { get; set; }

    public int? Duration { get; set; }

    public int? OrderIdx { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual TblLesson Lession { get; set; } = null!;

    public virtual ICollection<TblLearningProgress> TblLearningProgresses { get; set; } = new List<TblLearningProgress>();
}
