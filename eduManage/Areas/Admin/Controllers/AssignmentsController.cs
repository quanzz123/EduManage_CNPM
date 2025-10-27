
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
                string savedFileUrl = SaveFile(assignment.FileUpload);
                var newAssignment = new Assignment
                {
                    ClassId = assignment.ClassId,
                    Title = assignment.Title, 
                    Description = assignment.Description,
                    FileUrl = savedFileUrl,
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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment == null)
            {
                return NotFound();
            }
            var assignmentVM = new AssignmentVM
            {
                AssignmentId = assignment.AssignmentId,
                ClassId = assignment.ClassId,
                Title = assignment.Title,
                Description = assignment.Description,
                FileUrl = assignment.FileUrl,
                Deadline = assignment.Deadline,
                CreatedBy = 3,
                CreatedDate = assignment.CreatedDate,
                IsActive = assignment.IsActive
            };
            return View(assignmentVM);
        }
        [HttpPost]
        public IActionResult Edit(AssignmentVM assignmentVM)
        {

            if (!ModelState.IsValid)
            {
                return View(assignmentVM);
            }

            var assignment = _context.Assignments.Find(assignmentVM.AssignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            // Nếu có file mới được upload
            if (assignmentVM.FileUpload != null)
            {
                string newFileUrl = SaveFile(assignmentVM.FileUpload);
                assignment.FileUrl = newFileUrl;
            }

            // Cập nhật các trường khác
            assignment.Title = assignmentVM.Title;
            assignment.Description = assignmentVM.Description;
            assignment.Deadline = assignmentVM.Deadline;
            //assignment.IsActive = assignmentVM.IsActive;
            assignment.ModifyDate = DateTime.Now;

            _context.Assignments.Update(assignment);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = assignment.ClassId });
        }
        private string SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Thư mục lưu file trong wwwroot/uploads
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            
            return "/uploads/" + fileName;
        }

    }
}
