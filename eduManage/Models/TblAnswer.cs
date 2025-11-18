using System;
using System.Collections.Generic;

namespace eduManage.Models;

public partial class TblAnswer
{
    public int AnswerId { get; set; }

    public int QuestionId { get; set; }

    public string Content { get; set; } = null!;

    public bool IsCorect { get; set; }

    public virtual TblQuestion Question { get; set; } = null!;

    public virtual ICollection<TblStudentAnswer> TblStudentAnswers { get; set; } = new List<TblStudentAnswer>();
}
