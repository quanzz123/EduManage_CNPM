using eduManage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            public IActionResult Create()
            {
                var teachers = (from t in _context.TblUsers
                                .Where(u => u.RoleId == 2)
                                select new SelectListItem()
                                {
                                    Text = t.FullName,
                                    Value = t.UserId.ToString()
                                }
                                ).ToList();
                teachers.Insert(0, new SelectListItem()
                {
                    Text = "--Select Teacher--",
                    Value = "0"
                });
                ViewBag.TeacherList = teachers;
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> Create(TblClass? cls)
            {
                if (ModelState.IsValid)
                {
                    cls.CreateDate = DateTime.Now;
                    _context.TblClasses.Add(cls);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(cls);
            }
            public IActionResult Edit(int id)
            {
                var cls = _context.TblClasses.Find(id);
                ViewBag.TeacherList = new SelectList(_context.TblUsers.Where(u => u.RoleId == 2), "UserId", "FullName");
                return View(cls);
            }
            [HttpPost]
            public IActionResult Edit(TblClass model)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

            ViewBag.TeacherList = new SelectList(_context.TblUsers.Where(u => u.RoleId == 2), "UserId", "FullName");
            return View(model);
            }
        }
    }
