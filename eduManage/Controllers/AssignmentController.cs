using Microsoft.AspNetCore.Mvc;
using eduManage.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace eduManage.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly EdumanageContext _context;
        private readonly IWebHostEnvironment _environment;

        public AssignmentController(EdumanageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Hiển thị danh sách bài tập
        public IActionResult Index()
        {
            var assignments = _context.Assignments
                .OrderByDescending(a => a.Deadline)
                .ToList();

            return View(assignments);
        }

        // Xem chi tiết bài tập
        public IActionResult Details(int id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.AssignmentId == id);
            if (assignment == null)
            {
                return NotFound();
            }

            // Lấy bài nộp gần nhất theo đúng tên cột
            var latestSubmission = _context.Submissions
                .Where(s => s.AssignmentId == id)
                .OrderByDescending(s => s.SubmitDate)
                .FirstOrDefault();

            ViewBag.LatestSubmission = latestSubmission;
            return View(assignment);
        }


        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(int assignmentId, string submissionText, string comments, IFormFile submissionFile)
        {
            try
            {
                var studentId = HttpContext.Session.GetInt32("StudentId");
                if (studentId == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);
                if (assignment == null)
                {
                    TempData["ErrorMessage"] = "Bài tập không tồn tại!";
                    return RedirectToAction("Index");
                }

                string filePath = null;
                if (submissionFile != null && submissionFile.Length > 0)
                {
                    if (submissionFile.Length > 10 * 1024 * 1024)
                    {
                        TempData["ErrorMessage"] = "File không được vượt quá 10MB!";
                        return RedirectToAction("Details", new { id = assignmentId });
                    }

                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "submissions");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{studentId}_{assignmentId}_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(submissionFile.FileName)}";
                    filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await submissionFile.CopyToAsync(stream);
                    }

                    filePath = $"/submissions/{fileName}";
                }

                var submission = new Submission
                {
                    AssignmentId = assignmentId,
                    StudentId = studentId.Value,
                    FileUrl = filePath,
                    SubmitDate = DateTime.Now,
                    Score = null,
                    Feedback = comments,
                    Status = "Đã nộp"
                };

                _context.Submissions.Add(submission);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Nộp bài thành công!";
                return RedirectToAction("Details", new { id = assignmentId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi nộp bài: " + ex.Message;
                return RedirectToAction("Details", new { id = assignmentId });
            }
        }

        // Xem lịch sử nộp bài
        public IActionResult SubmissionHistory(int assignmentId)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var submissions = _context.Submissions
                .Where(s => s.AssignmentId == assignmentId && s.StudentId == studentId)
                .OrderByDescending(s => s.SubmitDate)
                .ToList();

            ViewBag.AssignmentId = assignmentId;
            return View(submissions);
        }
    }
}
