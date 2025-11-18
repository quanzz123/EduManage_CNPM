using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblQuiz
{
    public int QuizId { get; set; }

    public int ClassId { get; set; }

    public string Title { get; set; } = null!;

    public string? Descriptions { get; set; }

    public int? Duration { get; set; }

    public DateTime? CreateTime { get; set; }

    public bool? Isactive { get; set; }

    public virtual TblClass Class { get; set; } = null!;

    public virtual ICollection<TblQuestion> TblQuestions { get; set; } = new List<TblQuestion>();

    public virtual ICollection<TblQuizAttempt> TblQuizAttempts { get; set; } = new List<TblQuizAttempt>();

    public virtual ICollection<TblStudentAnswer> TblStudentAnswers { get; set; } = new List<TblStudentAnswer>();
}
