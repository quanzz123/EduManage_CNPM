using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblLearningProgress
{
    public int ProgressId { get; set; }

    public int UserId { get; set; }

    public int LessonId { get; set; }

    public decimal? CompletionRate { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime? LastAccessTime { get; set; }

    public int? TotalTimeSpent { get; set; }

    public int LastContentId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual TblLessionContent LastContent { get; set; } = null!;

    public virtual TblLesson Lesson { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
