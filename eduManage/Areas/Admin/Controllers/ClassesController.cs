using eduManage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var classList = _context.TblClasses.Include(m => m.Teacher).OrderBy(m => m.ClassId).ToList();

            return View(classList);
        }
    }
}
