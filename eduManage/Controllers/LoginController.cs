using Microsoft.AspNetCore.Mvc;
using eduManage.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace eduManage.Controllers
{
    public class LoginController : Controller
    {
        private readonly EdumanageContext _context;

        public LoginController(EdumanageContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Nếu đã đăng nhập, chuyển hướng đến dashboard
            var studentName = HttpContext.Session.GetString("StudentName");
            if (!string.IsNullOrEmpty(studentName))
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var student = _context.TblUsers
                .FirstOrDefault(u => u.Email == email && u.PassworkHash == password && u.RoleId == 3);

            if (student != null)
            {
                // Lưu thông tin vào Session
                HttpContext.Session.SetInt32("StudentId", student.UserId);
                HttpContext.Session.SetString("StudentName", student.FullName);
                HttpContext.Session.SetString("StudentEmail", student.Email);

                return RedirectToAction("Dashboard", "Student");
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng.";
            return View();
        }

        public IActionResult Dashboard()
        {
            var studentName = HttpContext.Session.GetString("StudentName");
            if (string.IsNullOrEmpty(studentName))
            {
                return RedirectToAction("Login");
            }

            ViewBag.StudentName = studentName;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}