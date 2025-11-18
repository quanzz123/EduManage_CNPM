using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using eduManage.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly EdumanageContext _context;

        public AssignmentController(EdumanageContext context)
        {
            _context = context;
        }

        // Danh sách bài tập
        public IActionResult Index()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            // Lấy danh sách lớp của sinh viên
            var studentClasses = _context.TblClassMembers
                .Where(m => m.UserId == studentId)
                .Select(m => m.ClassId)
                .ToList();

            // Lấy bài tập từ các lớp đó
            var assignments = _context.Assignments
                .Where(a => studentClasses.Contains(a.ClassId) && a.IsActive == true)
                .Include(a => a.Class)
                .OrderByDescending(a => a.Deadline)
                .ToList();

            // Kiểm tra trạng thái nộp bài
            var assignmentIds = assignments.Select(a => a.AssignmentId).ToList();
            var submissions = _context.Submissions
                .Where(s => s.StudentId == studentId && assignmentIds.Contains(s.AssignmentId))
                .ToList();

            ViewBag.Submissions = submissions;
            return View(assignments);
        }

        // Chi tiết bài tập
        public IActionResult Details(int id)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            var assignment = _context.Assignments
                .Include(a => a.Class)
                .FirstOrDefault(a => a.AssignmentId == id && a.IsActive == true);

            if (assignment == null)
            {
                TempData["ErrorMessage"] = "Bài tập không tồn tại";
                return RedirectToAction("Index");
            }

            // Kiểm tra sinh viên có trong lớp không
            var isInClass = _context.TblClassMembers
                .Any(m => m.UserId == studentId && m.ClassId == assignment.ClassId);

            if (!isInClass)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập bài tập này";
                return RedirectToAction("Index");
            }

            // Lấy bài nộp gần nhất
            var latestSubmission = _context.Submissions
                .Where(s => s.AssignmentId == id && s.StudentId == studentId)
                .OrderByDescending(s => s.SubmitDate)
                .FirstOrDefault();

            ViewBag.LatestSubmission = latestSubmission;
            return View(assignment);
        }

        // Nộp bài tập
        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(int assignmentId, string submissionText, string comments, IFormFile submissionFile)
        {
            // Validate bắt buộc có nội dung
            if (string.IsNullOrWhiteSpace(submissionText))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập nội dung bài làm";
                return RedirectToAction("Details", new { id = assignmentId });
            }

            // Validate file nếu có
            if (submissionFile != null && submissionFile.Length > 0)
            {
                // Kiểm tra kích thước file (10MB)
                if (submissionFile.Length > 10 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "File quá lớn. Kích thước tối đa là 10MB.";
                    return RedirectToAction("Details", new { id = assignmentId });
                }

                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".zip", ".rar", ".txt", ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(submissionFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["ErrorMessage"] = "Định dạng file không được hỗ trợ.";
                    return RedirectToAction("Details", new { id = assignmentId });
                }
            }
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            try
            {
                var assignment = await _context.Assignments.FindAsync(assignmentId);
                if (assignment == null)
                {
                    TempData["ErrorMessage"] = "Bài tập không tồn tại";
                    return RedirectToAction("Index");
                }

                // Kiểm tra hạn nộp
                if (assignment.Deadline.HasValue && assignment.Deadline.Value < DateTime.Now)
                {
                    TempData["ErrorMessage"] = "Đã quá hạn nộp bài";
                    return RedirectToAction("Details", new { id = assignmentId });
                }

                string fileUrl = null;
                if (submissionFile != null && submissionFile.Length > 0)
                {
                    // Lưu file (cần implement logic lưu file thực tế)
                    var fileName = $"{studentId}_{assignmentId}_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(submissionFile.FileName)}";
                    var filePath = Path.Combine("wwwroot/uploads", fileName);

                    // Tạo thư mục nếu chưa tồn tại
                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await submissionFile.CopyToAsync(stream);
                    }
                    fileUrl = $"/uploads/{fileName}";
                }

                var submission = new Submission
                {
                    AssignmentId = assignmentId,
                    StudentId = studentId.Value,
                    FileUrl = fileUrl,
                    SubmitDate = DateTime.Now,
                    Status = "Đã nộp",
                    Feedback = comments
                };

                _context.Submissions.Add(submission);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Nộp bài thành công!";
                return RedirectToAction("Details", new { id = assignmentId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi nộp bài: {ex.Message}";
                return RedirectToAction("Details", new { id = assignmentId });
            }
        }

        // Lịch sử nộp bài
        public IActionResult SubmissionHistory(int assignmentId)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToAction("Login", "Login");

            var submissions = _context.Submissions
                .Where(s => s.AssignmentId == assignmentId && s.StudentId == studentId)
                .OrderByDescending(s => s.SubmitDate)
                .ToList();

            ViewBag.AssignmentId = assignmentId;
            return View(submissions);
        }
    }
}