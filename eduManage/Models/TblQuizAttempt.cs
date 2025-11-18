using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblQuizAttempt
{
    public int AttemptId { get; set; }

    public int QuizId { get; set; }

    public int UserId { get; set; }

    public DateTime? Startime { get; set; }

    public DateTime? EndTime { get; set; }

    public double? Score { get; set; }

    public virtual TblQuiz Quiz { get; set; } = null!;

    public virtual ICollection<TblStudentAnswer> TblStudentAnswers { get; set; } = new List<TblStudentAnswer>();

    public virtual TblUser User { get; set; } = null!;
}
