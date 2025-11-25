using eduManage.Models;
using eduManage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuizController : Controller
    {
        private readonly EdumanageContext _context;
        public QuizController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var clasLis = _context.TblClasses.Where(t => t.TeacherId == 3).ToList();

            return View(clasLis);
        }

        public IActionResult Details(int id)
        {
            var quiz = _context.TblQuizzes.Where(c => c.ClassId == id && c.Isactive == true).ToList();
            ViewBag.classId = id;
            return View(quiz);
        }
        [HttpGet]
        public IActionResult Create(int id)
        {

            ViewBag.classId = id;
            return View();  
        }

        public IActionResult Create(QuizVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.classId = model.ClassId;
                return View(model);
            }

            var quiz = new TblQuiz
            {
                ClassId = model.ClassId,
                Title = model.Title,
                Duration = model.Duration,
                Descriptions = model.Descriptions,
                CreateTime = DateTime.Now,
                Isactive = true
            };

            _context.TblQuizzes.Add(quiz);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = model.ClassId });


            
        }
        [HttpGet]
        public IActionResult AddQuestion(int id)
        {
            var quiz = _context.TblQuizzes
                .Include(q => q.TblQuestions)
                    .ThenInclude(q => q.TblAnswers)
                .FirstOrDefault(q => q.QuizId == id);
            ViewBag.classId = id;
            return View(quiz);
        }

        [HttpPost]
        public IActionResult AddQuestion(TblQuestion question, List<string> answerContents, List<bool> isCorrectAnswers)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(question.Content))
                {
                    TempData["Error"] = "Nội dung câu hỏi không được để trống!";
                    return RedirectToAction("AddQuestion", new { quizId = question.QuizId });
                }

                if (answerContents == null || answerContents.Count == 0)
                {
                    TempData["Error"] = "Bạn phải nhập ít nhất 1 đáp án!";
                    return RedirectToAction("AddQuestion", new { quizId = question.QuizId });
                }

                
                _context.TblQuestions.Add(question);
                _context.SaveChanges();      

  
                for (int i = 0; i < answerContents.Count; i++)
                {
                    var answer = new TblAnswer
                    {
                        QuestionId = question.QuestionId,
                        Content = answerContents[i],
                        IsCorect = i < isCorrectAnswers.Count ? isCorrectAnswers[i] : false
                    };

                    _context.TblAnswers.Add(answer);
                }

                _context.SaveChanges();

                TempData["Success"] = "Thêm câu hỏi thành công!";
                return RedirectToAction("AddQuestion", "Quiz", new { id = question.QuizId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi hệ thống: " + ex.Message;
                return RedirectToAction("AddQuestion", new { quizId = question.QuizId });
            }
        }

    }
}
