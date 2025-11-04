using eduManage.Models;
using eduManage.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubmissionsController : Controller
    {
        private readonly EdumanageContext _context;
        private readonly IWebHostEnvironment _env;
        public SubmissionsController(EdumanageContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int id)
        {
            var submissions = _context.Submissions
                .Where(s => s.AssignmentId == id)
                .Select(s => new SubmissionVM
                {
                    SubmissionId = s.SubmissionId,
                    AssignmentId = s.AssignmentId,
                    StudentId = s.StudentId,
                    FileUrl = s.FileUrl,
                    SubmitDate = s.SubmitDate,
                    Score = s.Score,
                    Feedback = s.Feedback,
                    Status = s.Status
                }).ToList();

            ViewBag.AssignmentId = id;
            return View(submissions);
        }

        public IActionResult Download(int id)
        {
            var submission = _context.Submissions.Find(id);
            if (submission == null || string.IsNullOrEmpty(submission.FileUrl))
                return NotFound();

            // Dòng quan trọng
            var filePath = Path.Combine(_env.WebRootPath, submission.FileUrl.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
            {
                TempData["Error"] = "Tệp không tồn tại hoặc đã bị xóa!";
                return RedirectToAction("Index", new { assignmentId = submission.AssignmentId });
            }

            return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(filePath));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var submission = _context.Submissions.Find(id);
            var Student = _context.TblUsers.Find(submission.StudentId);
            var vm = new SubmissionVM
            {
                SubmissionId = submission.SubmissionId,
                AssignmentId = submission.AssignmentId,
                StudentId = submission.StudentId,
                FileUrl = submission.FileUrl,
                SubmitDate = submission.SubmitDate,
                Score = submission.Score,
                Feedback = submission.Feedback,
                Status = submission.Status,
                StudientName = Student.FullName
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(SubmissionVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var submission = _context.Submissions.Find(vm.SubmissionId);
            if (submission == null)
            {
                return NotFound();
            }
             
            submission.Score = vm.Score;
            submission.Feedback = vm.Feedback;
            submission.Status = "đã chấm";
            _context.Submissions.Update(submission);
            _context.SaveChanges();
            return RedirectToAction("Index", new { id = vm.AssignmentId });
        }
    }
}
