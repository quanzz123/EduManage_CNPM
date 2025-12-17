using Microsoft.AspNetCore.Mvc;
using eduManage.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using eduManage.Utilities;

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
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId != null)
                return RedirectToAction("Dashboard", "Student");

            // Đăng nhập tự động cho student1@edu.vn
            //var autoStudent = _context.TblUsers
            //    .FirstOrDefault(u => u.Email == "student2@edu.vn" && u.PassworkHash == "123456" && u.RoleId == 3);

            //if (autoStudent != null)
            //{
            //    HttpContext.Session.SetInt32("StudentId", autoStudent.UserId);
            //    HttpContext.Session.SetString("StudentName", autoStudent.FullName);
            //    HttpContext.Session.SetString("StudentEmail", autoStudent.Email);

            //    return RedirectToAction("Dashboard", "Student");
            //}
            // set role cho thành học viên
            Functions._RoleId = 3; 

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var student = _context.TblUsers
                .FirstOrDefault(u => u.Email == email && u.RoleId == 3);
            
            bool isPasswordValid = Utilities.Functions.VerifyPassword(password, student.PassworkHash );
            if (!isPasswordValid)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng.";
                return View();
            }
            if (student != null)
            {
                HttpContext.Session.SetInt32("StudentId", student.UserId);
                HttpContext.Session.SetString("StudentName", student.FullName);
                HttpContext.Session.SetString("StudentEmail", student.Email);

                return RedirectToAction("Dashboard", "Student");
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng.";
            // set role cho thành học viên
            Functions._RoleId = 3;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Functions._RoleId = 0;
            return RedirectToAction("Login", "Login");
        }
    }
}