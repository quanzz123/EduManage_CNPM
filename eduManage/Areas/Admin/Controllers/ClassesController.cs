using eduManage.Models;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]     
    public class ClassesController : Controller
    {
        private readonly EdumanageContext _context;
        public ClassesController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
