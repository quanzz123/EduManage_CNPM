using Microsoft.AspNetCore.Mvc;
using eduManage.Models;
using System.Linq;

namespace eduManage.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly EdumanageContext _context;

        public AssignmentController(EdumanageContext context)
        {
            _context = context;
        }

        // Hiển thị toàn bộ bài tập
        public IActionResult Index()
        {
            var assignments = _context.Assignments
                                      .OrderByDescending(a => a.Deadline)
                                      .ToList();

            return View(assignments);

            /*int studentId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            var assignments = (from a in _context.Assignments
                               join c in _context.TblClasses on a.ClassId equals c.ClassId
                               join m in _context.TblClassMembers on c.ClassId equals m.ClassId
                               where m.UserId == studentId && a.IsActive == true
                               select a).ToList();

            return View(assignments);*/
        }

        // Xem chi tiết bài tập
        public IActionResult Details(int id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.AssignmentId == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }
        
    }
}
