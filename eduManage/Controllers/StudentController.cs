using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using eduManage.Models;
using System.Linq;

namespace eduManage.Controllers
{
    public class StudentController : Controller
    {
        private readonly EdumanageContext _context;

        public StudentController(EdumanageContext context)
        {
            _context = context;
        }

        // 🏠 Trang tổng quan sinh viên
        public IActionResult Dashboard()
        {
            var studentName = HttpContext.Session.GetString("StudentName");
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            // Lấy các lớp học mà sinh viên này đang tham gia
            var classes = (from m in _context.TblClassMembers
                           join c in _context.TblClasses on m.ClassId equals c.ClassId
                           where m.UserId == studentId && c.IsActive == true
                           select c).ToList();

            ViewBag.StudentName = studentName;
            return View(classes);
        }

        // 📘 Danh sách bài học trong lớp
        public IActionResult Lessons(int classId)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            var lessons = _context.TblLessons
                .Where(l => l.ClassId == classId && l.IsActive == true)
                .OrderBy(l => l.OrderIdx)
                .ToList();

            ViewBag.ClassInfo = _context.TblClasses.FirstOrDefault(c => c.ClassId == classId);
            return View(lessons);
        }

        // ▶️ Xem nội dung bài học (video / tài liệu)
        public IActionResult LessonContent(int lessonId)
        {
            var contents = _context.TblLessionContents
                .Where(c => c.LessionId == lessonId)
                .OrderBy(c => c.OrderIdx)
                .ToList();

            ViewBag.Lesson = _context.TblLessons.FirstOrDefault(l => l.LessonId == lessonId);
            return View(contents);
        }

        // 📊 Cập nhật tiến độ học
        [HttpPost]
        public IActionResult UpdateProgress(int lessonId, decimal completionRate)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return Json(new { success = false });

            var progress = _context.TblLearningProgresses
                .FirstOrDefault(p => p.UserId == studentId && p.LessonId == lessonId);

            if (progress == null)
            {
                progress = new TblLearningProgress
                {
                    UserId = studentId.Value,
                    LessonId = lessonId,
                    CompletionRate = completionRate,
                    IsCompleted = completionRate >= 100,
                    UpdatedDate = DateTime.Now
                };
                _context.TblLearningProgresses.Add(progress);
            }
            else
            {
                progress.CompletionRate = completionRate;
                progress.IsCompleted = completionRate >= 100;
                progress.UpdatedDate = DateTime.Now;
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}
