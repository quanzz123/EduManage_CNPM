using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblQuestion
{
    public int QuestionId { get; set; }

    public int QuizId { get; set; }

    public string Content { get; set; } = null!;

    public int? Index { get; set; }

    public virtual TblQuiz Quiz { get; set; } = null!;

    public virtual ICollection<TblAnswer> TblAnswers { get; set; } = new List<TblAnswer>();
}
