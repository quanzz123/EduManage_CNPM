using eduManage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AcountsController : Controller
    {
        private readonly EdumanageContext _context;
        public AcountsController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var accList = _context.TblUsers.Include(m => m.Role).OrderBy(m => m.UserId).ToList();
            return View(accList);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {

            if(id == null || id == 0)
            {
                return NotFound();
            }
             var acc = _context.TblUsers.Find(id);
            if (acc == null)
            {
                return NotFound();
            }
            
            return View(acc);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var acc = _context.TblUsers.Find(id);
            if (acc == null)
            {
                return NotFound();
            }
            acc.IsActive = false;
            _context.TblUsers.Update(acc);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
