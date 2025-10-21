using eduManage.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Create(int id)
        {
            ViewBag.ClassId = id;
            return View();
        }
    }
}
