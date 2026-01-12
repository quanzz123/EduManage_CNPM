using eduManage.Models;
using eduManage.Utilities;
using eduManage.ViewModels;
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
                if (!Functions.IsLogin())
                {
                    return RedirectToAction("Index", "Login", new { area = "Admin" });
                }
                if (!Functions.CheckRole(1))
                {
                    return RedirectToAction("AccessDenied", "Error");
                }
            var classList = _context.TblClasses.Include(m => m.Teacher).OrderBy(m => m.ClassId).ToList();

                return View(classList);
            }

        public IActionResult MyClasses()
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
           

            var userId = Functions._UserId;
            var classList = _context.TblClasses
                .Where(c => c.TeacherId == userId)
                .Include(m => m.Teacher)
                .OrderBy(m => m.ClassId)
                .ToList();
            return View(classList);
        }
        public IActionResult Create()
            {
                if(Functions._RoleId != 1)
                {
                    return RedirectToAction("AccessDenied", "Error", new { area = "Admin" });
                }
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
            public IActionResult Create(ClassesVM model)
            {
                if (Functions._RoleId != 1)
                {
                    return RedirectToAction("AccessDenied", "Error", new { area = "Admin" });
                }

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("⚠️ ModelState is INVALID. Showing validation errors:");

                    foreach (var state in ModelState)
                    {
                        string fieldName = state.Key;
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"❌ Field: {fieldName} | Error: {error.ErrorMessage}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("✅ ModelState is VALID. Proceeding to save...");
                }
                if (ModelState.IsValid)
                {
                        var newClass = new TblClass
                        {
                            ClassName = model.ClassName,
                            Description = model.Description,
                            Subject = model.Subject,
                            TeacherId = model.TeacherId ?? 0,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            Schedule = model.Schedule,
                            IsActive = model.IsActive ?? true,
                            Image = model.Image ?? "NaN",
                            CreateDate = DateTime.Now,
                            MaxStudents = model.MaxStudents
                        };
                        _context.TblClasses.Add(newClass);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                
                    }
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
            return View(model);
            
            }
            [HttpGet]    
            public IActionResult Edit(int id)
            {
                if (Functions._RoleId != 1)
                {
                    return RedirectToAction("AccessDenied", "Error", new { area = "Admin" });
                }
            var cls = _context.TblClasses.Find(id);
                if (cls == null)
                {
                    return View("Error");
                }
                var vm = new ClassesVM
                { 
                    ClassId = cls.ClassId,        
                    ClassName = cls.ClassName,
                    Description = cls.Description,
                    Subject = cls.Subject,
                    TeacherId = cls.TeacherId,
                    StartDate = cls.StartDate,
                    EndDate = cls.EndDate,
                    Schedule = cls.Schedule,
                    IsActive = cls.IsActive ?? true,
                    MaxStudents = cls.MaxStudents

                };
                ViewBag.TeacherList = new SelectList(
                    _context.TblUsers.Where(u => u.RoleId == 2),
                    "UserId", "FullName", cls.TeacherId
                );
            return View(vm);
            }
            [HttpPost]
            public IActionResult Edit(ClassesVM model)
            {
           
            if (ModelState.IsValid)
                {

                    var oldClass = _context.TblClasses.FirstOrDefault(c => c.ClassId == model.ClassId);
                    if (oldClass == null)
                    {
                        return NotFound();
                    }
                    oldClass.ClassName = model.ClassName;
                    oldClass.Description = model.Description;
                    oldClass.Subject = model.Subject;
                    oldClass.TeacherId = model.TeacherId ?? 0;
                    oldClass.StartDate = model.StartDate;
                    oldClass.EndDate = model.EndDate;
                    oldClass.Schedule = model.Schedule;
                    oldClass.MaxStudents = model.MaxStudents;
                    oldClass.IsActive = model.IsActive ?? true;
                    oldClass.ModifedDate = DateTime.Now;
                    _context.Update(oldClass);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

            ViewBag.TeacherList = new SelectList(
            _context.TblUsers.Where(u => u.RoleId == 2),
            "UserId", "FullName", model.TeacherId
            );                                                                    
            return View(model);
            }
        }
    }
