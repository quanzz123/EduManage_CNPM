using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblStudentAnswer
{
    public int StudentAswerId { get; set; }

    public int Userid { get; set; }

    public int AttempId { get; set; }

    public int AnswerId { get; set; }

    public int QuizId { get; set; }

    public virtual TblAnswer Answer { get; set; } = null!;

    public virtual TblQuizAttempt Attemp { get; set; } = null!;

    public virtual TblQuiz Quiz { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
