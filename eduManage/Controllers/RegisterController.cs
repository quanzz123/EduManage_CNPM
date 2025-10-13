using eduManage.Models;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Controllers
{
    public class RegisterController : Controller
    {
        private readonly EdumanageContext _context;
        public RegisterController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
