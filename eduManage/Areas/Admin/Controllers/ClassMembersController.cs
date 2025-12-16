using eduManage.Models;
using eduManage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using eduManage.Utilities;

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
        public IActionResult Index(int? id)
        {
            // Join 3 bảng: ClassMembers + Users + Classes
            var query = from cm in _context.TblClassMembers
                        join u in _context.TblUsers on cm.UserId equals u.UserId
                        join c in _context.TblClasses on cm.ClassId equals c.ClassId
                        select new TblClassMember
                        {
                            MemberId = cm.MemberId,
                            ClassId = cm.ClassId,
                            JoinDate = cm.JoinDate,
                            Status = cm.Status,
                            Progress = cm.Progress,
                            FinalScore = cm.FinalScore,
                            Note = cm.Note,
                            UserId = cm.UserId,
                            User = u,
                            Class = c // ⚡ Gán luôn Class để hiển thị
                        };

            if (id != null)
            {
                query = query.Where(m => m.ClassId == id);
                ViewBag.ClassId = id;
            }

            var memberList = query.ToList();
            return View(memberList);
        }
        // GET: admin/ClassMembers/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var member = _context.TblClassMembers.Find(id);
            if (member == null)
            {
                return NotFound();
            }

            ViewBag.StudentList = new SelectList(_context.TblUsers, "UserId", "FullName", member.UserId);
            ViewBag.ClassList = new SelectList(_context.TblClasses, "ClassId", "ClassName", member.ClassId);
            return View(member);
        }

        // POST: admin/ClassMembers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TblClassMember model)
        {
            if (id != model.MemberId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật thành viên thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật dữ liệu!");
                }
            }

            // Reload dropdown nếu có lỗi
            ViewBag.StudentList = new SelectList(_context.TblUsers, "UserId", "FullName", model.UserId);
            ViewBag.ClassList = new SelectList(_context.TblClasses, "ClassId", "ClassName", model.ClassId);

            return View(model);
        }


        /*        public IActionResult Edit(int id)
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
                }*/

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
                var classes = await _context.TblClasses.FindAsync(vm.ClassId);
                String classcođe = StringHelper.ToAbbreviation(classes.ClassName);
                String createDate = classes.CreateDate.HasValue ? classes.CreateDate.Value.ToString("yyyyMMdd") : "N/A";
                String userID = vm.UserId.ToString();
                String msv = $"{classcođe}{createDate}{userID}";
                var member = new TblClassMember
                {
                    ClassId = vm.ClassId,
                    UserId = vm.UserId,
                    JoinDate = vm.JoinDate,
                    Status = vm.Status,
                    Progress = vm.Progress,
                    FinalScore = vm.FinalScore,
                    Note = msv
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
