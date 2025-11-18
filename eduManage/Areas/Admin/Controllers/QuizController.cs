using eduManage.Models;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Areas.Admin.Controllers
{
    public class QuizController : Controller
    {
        private readonly EdumanageContext _context;
        public QuizController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
