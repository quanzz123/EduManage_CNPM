using eduManage.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
