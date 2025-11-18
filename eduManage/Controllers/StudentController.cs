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

        // Trang tổng quan sinh viên
        public IActionResult Dashboard()
        {
            var studentName = HttpContext.Session.GetString("StudentName");
            var studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
                return RedirectToAction("Login", "Login");

            // Lấy danh sách lớp mà sinh viên đang tham gia
            var classes = _context.TblClassMembers
                .Where(m => m.UserId == studentId)
                .Join(_context.TblClasses,
                      m => m.ClassId,
                      c => c.ClassId,
                      (m, c) => c)
                .Where(c => c.IsActive == true)
                .ToList();

            // Lấy thông tin giáo viên
            var teacherIds = classes.Select(c => c.TeacherId).Distinct();
            var teachers = _context.TblUsers
                .Where(u => teacherIds.Contains(u.UserId))
                .ToDictionary(u => u.UserId, u => u.FullName);

            // Lấy số lượng bài học cho mỗi lớp
            var lessonCounts = new Dictionary<int, int>();
            foreach (var classItem in classes)
            {
                var count = _context.TblLessons
                    .Count(l => l.ClassId == classItem.ClassId && l.IsActive == true);
                lessonCounts[classItem.ClassId] = count;
            }

            ViewBag.StudentName = studentName;
            ViewBag.Teachers = teachers;
            ViewBag.LessonCounts = lessonCounts;

            return View(classes);
        }

        // Danh sách bài học theo lớp
        public IActionResult Lessons(int classId)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            // KIỂM TRA classId CÓ HỢP LỆ KHÔNG
            if (classId <= 0)
            {
                TempData["Error"] = "ClassId không hợp lệ";
                return RedirectToAction("Dashboard");
            }

            Console.WriteLine($"=== DEBUG Lessons ===");
            Console.WriteLine($"ClassId nhận được: {classId}");
            Console.WriteLine($"StudentId: {studentId}");

            try
            {
                // Kiểm tra class có tồn tại không
                var classInfo = _context.TblClasses.FirstOrDefault(c => c.ClassId == classId);
                if (classInfo == null)
                {
                    Console.WriteLine($"ClassId {classId} không tồn tại trong database!");
                    TempData["Error"] = "Lớp học không tồn tại";
                    return RedirectToAction("Dashboard");
                }

                // Kiểm tra sinh viên có trong lớp không
                var classMember = _context.TblClassMembers
                    .FirstOrDefault(m => m.UserId == studentId && m.ClassId == classId);

                if (classMember == null)
                {
                    TempData["Error"] = "Bạn không có quyền truy cập lớp học này";
                    return RedirectToAction("Dashboard");
                }

                Console.WriteLine($"Class tìm thấy: {classInfo.ClassName}");

                var lessons = _context.TblLessons
                    .Where(l => l.ClassId == classId && l.IsActive == true)
                    .OrderBy(l => l.OrderIdx)
                    .ToList();

                Console.WriteLine($"Số bài học tìm thấy: {lessons.Count}");

                // Lấy danh sách progress
                var lessonIds = lessons.Select(l => l.LessonId).ToList();
                var progressList = _context.TblLearningProgresses
                    .Where(p => p.UserId == studentId && lessonIds.Contains(p.LessonId))
                    .ToList();

                ViewBag.ClassInfo = classInfo;
                ViewBag.Progress = progressList;

                return View(lessons);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách bài học";
                return RedirectToAction("Dashboard");
            }
        }

        // Nội dung bài học
        public IActionResult LessonContent(int lessonId)
        {
            var contents = _context.TblLessionContents
                .Where(c => c.LessionId == lessonId)
                .OrderBy(c => c.OrderIdx)
                .ToList();

            ViewBag.Lesson = _context.TblLessons.FirstOrDefault(l => l.LessonId == lessonId);
            return View(contents);
        }

        // Cập nhật tiến độ học
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
                    LastAccessTime = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                _context.TblLearningProgresses.Add(progress);
            }
            else
            {
                progress.CompletionRate = completionRate;
                progress.IsCompleted = completionRate >= 100;
                progress.LastAccessTime = DateTime.Now;
                progress.UpdatedDate = DateTime.Now;
            }

            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
