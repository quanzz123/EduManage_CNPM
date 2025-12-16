using eduManage.Models;
using eduManage.Utilities;
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
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            if (!Functions.CheckRole(1))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            var accList = _context.TblUsers.Include(m => m.Role).OrderBy(m => m.UserId).ToList();
            return View(accList);
        }
        public IActionResult Create()
        {
            if(!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            if (!Functions.CheckRole(1))
            {
                return RedirectToAction("AccessDenied", "Error");
            }
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
        public async Task<IActionResult> Create(TblUser? acc)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            if (!Functions.CheckRole(1))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (ModelState.IsValid)
            {
                acc.PassworkHash = Functions.HashPassword(acc.PassworkHash);
                acc.CreateDate = DateTime.Now;
                acc.IsActive = true;
                _context.TblUsers.Add(acc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
            return View(acc);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            if (!Functions.CheckRole(1))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var acc = _context.TblUsers.Find(id);
            if (acc == null)
            {
                return NotFound();
            }
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
                Value = string.Empty
            });
            ViewBag.RoleList = role;
            return View(acc);
        }
        [HttpPost]  
        public IActionResult Edit(TblUser? acc)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            if (!Functions.CheckRole(1))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (ModelState.IsValid)
            {
                //acc.PassworkHash = Functions.HashPassword(acc.PassworkHash);
                //Console.WriteLine(acc.PassworkHash);
                //_context.TblUsers.Update(acc);
                //_context.SaveChanges();
                
                //return RedirectToAction(nameof(Index));
                var existingUser = _context.TblUsers.FirstOrDefault(u => u.UserId == acc.UserId);
                if(existingUser == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(acc.PassworkHash) && acc.PassworkHash != existingUser.PassworkHash)
                {
                    existingUser.PassworkHash = Functions.HashPassword(acc.PassworkHash);
                }
                existingUser.UserName = acc.UserName;
                existingUser.FullName = acc.FullName;
                existingUser.Email = acc.Email;
                existingUser.Phone = acc.Phone;
                existingUser.Address = acc.Address;
                existingUser.RoleId = acc.RoleId;
                //debug
                Console.WriteLine("Updated User: " + existingUser.UserId);
                Console.WriteLine("Updated User: " + existingUser.UserName);
                Console.WriteLine("Updated User: " + existingUser.FullName);
                Console.WriteLine("Updated User: " + existingUser.Email);
                Console.WriteLine("Updated User: " + existingUser.Phone);
                Console.WriteLine("Updated User: " + existingUser.Address);
                Console.WriteLine("Updated User: " + existingUser.RoleId);
                Console.WriteLine("Updated User: " + existingUser.PassworkHash);
                _context.SaveChanges();
               
                return RedirectToAction(nameof(Index));
            }
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
                Value = string.Empty
            });
            ViewBag.RoleList = role;
            return View(acc);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            if (!Functions.CheckRole(1))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (id == null || id == 0)
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
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            if (!Functions.CheckRole(1))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

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
