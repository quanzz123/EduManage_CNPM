using eduManage.Models;
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

    }
}
