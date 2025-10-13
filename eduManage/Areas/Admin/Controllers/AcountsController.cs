using eduManage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Create()
        {
            var role = (from r in _context.TblRoles
                        select new SelectListItem()
                        {
                            Text = r.RoleName,
                            Value = r.RoleId.ToString()
                        }
                        ).ToList();
            role.Insert(0, new SelectListItem()
            {
                Text = "--Select Role--",
                Value = "0"
            });
            ViewBag.RoleList = role;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TblUser acc)
        {
            if (acc.RoleId == 0)
            {
                ModelState.AddModelError("RoleId", "The Role field is required.");
            }

            if (ModelState.IsValid)
            {
                acc.CreateDate = DateTime.Now;
                acc.IsActive = true;
                _context.TblUsers.Add(acc);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            var role = _context.TblRoles
                .Select(r => new SelectListItem
                {
                    Text = r.RoleName,
                    Value = r.RoleId.ToString()
                }).ToList();

            role.Insert(0, new SelectListItem
            {
                Text = "--Select Role--",
                Value = "0"
            });

            ViewBag.RoleList = role;
            return View(acc);
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
