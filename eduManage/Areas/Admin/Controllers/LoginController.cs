using eduManage.Models;
using eduManage.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class LoginController : Controller
    {
        private readonly EdumanageContext _context;
        public LoginController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(String email, string password)
        {
            var user = await _context.TblUsers.Include(r => r.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "Sai ten dang nhap hoac mat khau");
                return View();
            }
            bool isPasswordValid = Utilities.Functions.VerifyPassword(password, user.PassworkHash);
            if (!isPasswordValid)
            {
                ModelState.AddModelError("", "Sai ten dang nhap hoac mat khau");
                return View();
            }
            Functions._UserId = user.UserId;
            Functions._UserName = user.UserName;
            Functions._FullName = user.FullName;
            Functions._Email = user.Email;
            Functions._RoleId = user.RoleId ?? 0;
            Functions._RoleName = user.Role?.RoleName ?? String.Empty;
            return RedirectToAction("Index", "Home", new { area = "Admin" });
            
        }
    }
}
