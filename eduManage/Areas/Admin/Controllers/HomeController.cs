using eduManage.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        public IActionResult Index()

        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
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
