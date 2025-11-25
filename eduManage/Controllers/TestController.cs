using eduManage.Models;
using eduManage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Controllers
{
    public class TestController : Controller
    {
        private readonly EdumanageContext _context;
        public TestController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");
            var cls = _context.TblClasses
                .Include(c => c.TblClassMembers)
                .Where(c => c.TblClassMembers
                .Any(m => m.UserId == studentId)).ToList();


            return View(cls);
        }

        public IActionResult QuizList(int id)
        {
            var userid = HttpContext.Session.GetInt32("StudentId");
            var data = (from q in _context.TblQuizzes
                        where q.ClassId == id 
                        select new QuizListVM
                        {
                            QuizId = q.QuizId,
                            Title = q.Title,
                            SubjectName = "Tiếng Anh", 
                            Duration = (int)q.Duration,
                            //Deadline = q.CreateTime.AddDays(7),
                            QuestionCount = _context.TblQuestions.Count(x => x.QuizId == q.QuizId),

                            IsDone = _context.TblQuizAttempts.Any(a => a.QuizId == q.QuizId && a.UserId == userid),

                            Score = _context.TblQuizAttempts
                                        .Where(a => a.QuizId == q.QuizId && a.UserId == userid)
                                        .Select(a => a.Score)
                                        .FirstOrDefault()
                        }).ToList();
            return View(data);
        }

        public IActionResult Start(int id)
        {
            var userId = HttpContext.Session.GetInt32("StudentId"); 

            var attempt = new TblQuizAttempt
            {
                QuizId = id,
                UserId = (int)userId,
                Startime = DateTime.Now
            };

            _context.TblQuizAttempts.Add(attempt);
            _context.SaveChanges();

            return RedirectToAction("DoQuiz", new { attemptId = attempt.AttemptId });
        }

        public IActionResult DoQuiz(int attemptId)
        {
            var attempt = _context.TblQuizAttempts.Find(attemptId);

            var quiz = _context.TblQuizzes
                .Include(q => q.TblQuestions)
                    .ThenInclude(q => q.TblAnswers)
                .FirstOrDefault(q => q.QuizId == attempt.QuizId);
            ViewBag.AttemptId = attemptId;
            return View(quiz);
        }

        [HttpPost]
        public IActionResult Submit(int QuizId, int AttemptId, IFormCollection form)
        {
            var userId = HttpContext.Session.GetInt32("StudentId");

            var quiz = _context.TblQuizzes
                .Include(q => q.TblQuestions)
                    .ThenInclude(q => q.TblAnswers)
                .FirstOrDefault(q => q.QuizId == QuizId);

            int total = quiz.TblQuestions.Count;
            int correct = 0;

            foreach (var q in quiz.TblQuestions)
            {
                string key = "q_" + q.QuestionId;
                if (form.ContainsKey(key))
                {
                    int chosenAnswerId = int.Parse(form[key]);

                    var studentAnswer = new TblStudentAnswer
                    {
                        Userid = (int)userId,
                        AttempId = AttemptId,
                        AnswerId = chosenAnswerId,
                        QuizId = QuizId
                    };
                    _context.TblStudentAnswers.Add(studentAnswer);

                    var answer = q.TblAnswers.First(a => a.AnswerId == chosenAnswerId);
                    if (answer.IsCorect) correct++;
                }
            }

            double score = (double)correct / total * 10;

            var attempt = _context.TblQuizAttempts.Find(AttemptId);
            attempt.EndTime = DateTime.Now;
            attempt.Score = score;

            _context.SaveChanges();

            return RedirectToAction("Result", new { quizid = QuizId });
        }
        public IActionResult Result(int quizid)
        {
            var attempt = _context.TblQuizAttempts
                .Include(a => a.Quiz)
                .FirstOrDefault(a => a.QuizId == quizid);
            return View(attempt);
        }

    }
}
