using eduManage.Models;
using eduManage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClassMembersController : Controller
    {
        private readonly EdumanageContext _context;
        public ClassMembersController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            var memberList = _context.TblClassMembers
                .Join(_context.TblUsers,
                      cm => cm.UserId,
                      u => u.UserId,
                      (cm, u) => new TblClassMember
                      {
                          MemberId = cm.MemberId,
                          ClassId = cm.ClassId,
                          JoinDate = cm.JoinDate,
                          Status = cm.Status,
                          Progress = cm.Progress,
                          FinalScore = cm.FinalScore,
                          Note = cm.Note,
                          UserId = cm.UserId,
                          User = u
                      })
                .Where(m => m.ClassId == id)
                .ToList();
            ViewBag.ClassId = id;
            return View(memberList);
        }
        public IActionResult Edit(int id)
        {
            var member = _context.TblClassMembers.Find(id);

            ViewBag.StudentList = new SelectList(
                _context.TblUsers.Where(u => u.RoleId == 3), // 3 = học viên
                "UserId", "FullName", member?.UserId
            );

            ViewBag.ClassList = new SelectList(
                _context.TblClasses,
                "ClassId", "ClassName", member?.ClassId
            );

            return View(member);
        }

        [HttpPost]
        public IActionResult Edit(TblClassMember model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Load lại danh sách khi có lỗi
            ViewBag.StudentList = new SelectList(
                _context.TblUsers.Where(u => u.RoleId == 3),
                "UserId", "FullName", model.UserId
            );

            ViewBag.ClassList = new SelectList(
                _context.TblClasses,
                "ClassId", "ClassName", model.ClassId
            );

            return View(model);
        }

        public IActionResult Create(int id)
        {
            var cls = _context.TblClasses.Find(id);
            if (cls == null) return NotFound();

            ViewBag.ClassId = id;
            ViewBag.ClassName = cls.ClassName;

            var students = _context.TblUsers
                .Where(u => u.RoleId == 3)
                .Select(u => new SelectListItem
                {
                    Text = u.FullName,
                    Value = u.UserId.ToString()
                })
                .ToList();

            ViewBag.StudentList = students;

            var vm = new ClassMemberVM
            {
                ClassId = id,
                JoinDate = DateTime.Now,
                Status = "Enrolled"
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassMemberVM vm)
        {
            if (ModelState.IsValid)
            {
                var member = new TblClassMember
                {
                    ClassId = vm.ClassId,
                    UserId = vm.UserId,
                    JoinDate = vm.JoinDate,
                    Status = vm.Status,
                    Progress = vm.Progress,
                    FinalScore = vm.FinalScore,
                    Note = vm.Note
                };

                _context.TblClassMembers.Add(member);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = vm.ClassId });
            }

            // Nếu có lỗi, load lại dữ liệu để view hiển thị đúng
            ViewBag.StudentList = _context.TblUsers
                .Where(u => u.RoleId == 3)
                .Select(u => new SelectListItem
                {
                    Text = u.FullName,
                    Value = u.UserId.ToString()
                })
                .ToList();

            return View(vm);
        }

    }
}
