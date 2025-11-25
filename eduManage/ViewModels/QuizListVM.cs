namespace eduManage.ViewModels
{
    public class QuizListVM
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string SubjectName { get; set; }   
        public int Duration { get; set; }
        public int QuestionCount { get; set; }
        public DateTime Deadline { get; set; }

        public bool IsDone { get; set; }
        public double? Score { get; set; }
    }
}
