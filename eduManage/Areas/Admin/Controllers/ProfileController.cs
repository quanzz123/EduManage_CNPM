using eduManage.Models;
using eduManage.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfileController : Controller
    {
        private readonly EdumanageContext _context;
        public ProfileController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var UserId = Functions._UserId;
            var user = _context.TblUsers
                   .Include(u => u.Role)
                   .FirstOrDefault(u => u.UserId == UserId);
            return View(user);
        }
    }
}
