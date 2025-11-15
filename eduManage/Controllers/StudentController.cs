using eduManage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // Trang tổng quan sinh viên
        public IActionResult Dashboard()
        {
            var studentName = HttpContext.Session.GetString("StudentName");
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            // Lấy danh sách lớp active của sinh viên
            var classes = _context.TblClassMembers
                .Where(m => m.UserId == studentId)
                .Join(_context.TblClasses,
                      m => m.ClassId,
                      c => c.ClassId,
                      (m, c) => c)
                .AsEnumerable()  // chuyển sang client để filter bool?
                .Where(c => c.IsActive.GetValueOrDefault())
                .ToList();

            ViewBag.StudentName = studentName;
            ViewBag.StudentId = studentId;

            return View(classes);
        }

        // Danh sách bài học trong lớp
        public IActionResult Lessons(int classId)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            // Lấy tất cả lessons của lớp
            var lessons = _context.TblLessons
                .Include(l => l.TblLessionContents)
                .Include(l => l.TblLearningProgresses)
                .Where(l => l.ClassId == classId)
                .AsEnumerable()  // chuyển sang client
                .Where(l => l.IsActive.GetValueOrDefault())  // filter nullable bool
                .OrderBy(l => l.OrderIdx)
                .ToList();

            var classInfo = _context.TblClasses.FirstOrDefault(c => c.ClassId == classId);

            ViewBag.ClassInfo = classInfo;
            ViewBag.StudentId = studentId;

            return View(lessons);
        }

        // Xem nội dung bài học
        public IActionResult LessonContent(int lessonId)
        {
<<<<<<< HEAD
=======
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

>>>>>>> 094386d1a1a01a0e43ad1b03c56e72832ca52cdc
            var contents = _context.TblLessionContents
                .Where(c => c.LessionId == lessonId)
                .OrderBy(c => c.OrderIdx)
                .ToList();

            var lesson = _context.TblLessons.FirstOrDefault(l => l.LessonId == lessonId);

            var progress = _context.TblLearningProgresses
                .FirstOrDefault(p => p.UserId == studentId && p.LessonId == lessonId);

            ViewBag.Lesson = lesson;
            ViewBag.Progress = progress;

            return View(contents);
        }
    }
}
