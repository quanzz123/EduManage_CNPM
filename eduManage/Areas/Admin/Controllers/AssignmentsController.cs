
using Microsoft.AspNetCore.Mvc;
using eduManage.Models;
using eduManage.ViewModels;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AssignmentsController : Controller
    {
        private readonly EdumanageContext _context;
        public AssignmentsController(EdumanageContext context)
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

            var exerciseLis = _context.Assignments.Where(a => a.ClassId == id).ToList();
            ViewBag.ClassId = id;
            return View(exerciseLis);
        }
        [HttpGet]
        public IActionResult Create(int id)
        {
            
            ViewBag.ClassId = id;
            return View();
        }
        [HttpPost]
        public IActionResult Create(AssignmentVM? assignment)
        {
            if (ModelState.IsValid)
            {
                var newAssignment = new Assignment
                {
                    ClassId = assignment.ClassId,
                    Title = assignment.Title, 
                    Description = assignment.Description,
                    FileUrl = assignment.FileUrl,
                    Deadline = assignment.Deadline,
                    CreatedBy = assignment.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                _context.Assignments.Add(newAssignment);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = assignment.ClassId });
            }
            return View(assignment);
        }
    }
}
