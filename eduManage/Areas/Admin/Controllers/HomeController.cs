using eduManage.Models;
using eduManage.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly EdumanageContext _context;
        public HomeController(EdumanageContext context)
        {
            _context = context;
        }

        public IActionResult Index()

        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            if(!Functions.CheckRole(1) && !Functions.CheckRole(2))
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            var UserID = Functions._UserId;
            var ClassList = _context.TblClasses.Where(t => t.TeacherId == UserID).ToList();
            ViewBag.ClassList = ClassList;
            return View();
        }

        public IActionResult Logout()
        {
            eduManage.Utilities.Functions._UserId = 0;
            eduManage.Utilities.Functions._UserName = string.Empty;
            eduManage.Utilities.Functions._Email = string.Empty;
            eduManage.Utilities.Functions._RoleId = 0;
            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }
    }
}
