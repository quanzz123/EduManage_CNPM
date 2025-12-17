using eduManage.Models;
using eduManage.Utilities;
using eduManage.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessionsController : Controller
    {
        private readonly EdumanageContext _context;
        public LessionsController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            if (!Functions.CheckRole(2))
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            var userId = Functions._UserId;
            var clasLis = _context.TblClasses.Where(t => t.TeacherId == userId).ToList();
            return View(clasLis);
        }

        public IActionResult Details(int id)
        {
            var lesson = _context.TblLessons.Where(c => c.ClassId == id && c.IsActive == true).ToList();
            ViewBag.classId = id;
            return View(lesson);
        }
        [HttpGet]
        public IActionResult Create(int id) {
            ViewBag.classId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(LessonsVM? lesson)
        {
            if(ModelState.IsValid)
            {
                var newLesson = new TblLesson
                {
                    ClassId = lesson.ClassId,
                    Title = lesson.Title,
                    Description = lesson.Description,
                    OrderIdx = lesson.OrderIdx,
                    CreateDate = DateTime.Now,
                    CreateBy = lesson.CreateBy,
                    IsActive = lesson.IsActive ?? true
                };
                _context.TblLessons.Add(newLesson);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = lesson.ClassId });
            }
            return View(lesson);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var lesson = _context.TblLessons.FirstOrDefault(l => l.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }
            var lessonVM = new LessonsVM
            {
                LessonId = lesson.LessonId,
                ClassId = lesson.ClassId,
                Title = lesson.Title,
                Description = lesson.Description,
                OrderIdx = lesson.OrderIdx,
                CreateDate = lesson.CreateDate,
                CreateBy = lesson.CreateBy,
                IsActive = lesson.IsActive
            };
            ViewBag.classId = id;
            return View(lessonVM);
        }
        [HttpPost]
        public IActionResult Edit(LessonsVM? lesson)
        {
            if (ModelState.IsValid)
            {
                var existingLesson = _context.TblLessons.FirstOrDefault(l => l.LessonId == lesson.LessonId);
                if (existingLesson == null)
                {
                    return NotFound();
                }
                existingLesson.ClassId = lesson.ClassId;
                existingLesson.Title = lesson.Title;
                existingLesson.Description = lesson.Description;
                existingLesson.OrderIdx = lesson.OrderIdx;
                existingLesson.CreateDate = lesson.CreateDate;
                existingLesson.CreateBy = lesson.CreateBy;
                existingLesson.IsActive = lesson.IsActive;
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = lesson.ClassId });
            }
            return View(lesson);
        }
    }
}
