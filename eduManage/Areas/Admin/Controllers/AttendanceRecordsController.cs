using eduManage.Models;
using eduManage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AttendanceRecordsController : Controller
    {
        private readonly EdumanageContext _context;
        public AttendanceRecordsController(EdumanageContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var claslist = _context.TblClasses.Where(c => c.TeacherId == 3).ToList();
            return View(claslist);
        }

        public IActionResult Details(int id)
        {
            var sessionList = _context.TblAttendanceSessions.Where(s => s.ClassId == id).Include(s=>s.Class).ToList();
            ViewBag.ClassId = id;
            return View(sessionList);
        }
        
        public IActionResult Create(int id)
        {
            var newSession = new TblAttendanceSession
            {
                ClassId = id,
                SessionDate = DateTime.Now,
                UserId = 3 // id test cho giao viên bằng 3
            };
            _context.TblAttendanceSessions.Add(newSession);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = newSession.ClassId });
        }
        public IActionResult Take(int id)
        {
            var session = _context.TblAttendanceSessions.FirstOrDefault(s => s.SessionsId == id);
            if (session == null)
            {
                return NotFound();
            }

            var existingRecords = _context.TblAttendanceRecords
                .Where(r => r.SessionId == id)
                .Include(r => r.User)
                .ToList();
            List<AttendanceItem> students;
            if (existingRecords.Any())
            {
                students = existingRecords
                    .Select(r => new AttendanceItem
                    {
                        UserId = r.UserId,
                        FullName = r.User.FullName,
                        Status = r.StatusId,
                        Note = r.Note
                    })
                    .ToList();
            }
            else
            {
                students = _context.TblClassMembers
                    .Where(x => x.ClassId == session.ClassId)
                    .Include(x => x.User)
                    .Select(x => new AttendanceItem
                    {
                        UserId = x.UserId ?? 0,
                        FullName = x.User.FullName,
                        Status = 1 
                    }).ToList();
            }
            var vm = new AttendanceViewModel
            {
                SessionId = id,
                classId = session.ClassId,
                Students = students
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Save(AttendanceViewModel model)
        {
            foreach (var stu in model.Students)
            {
                var record = _context.TblAttendanceRecords
                    .FirstOrDefault(r => r.SessionId == model.SessionId && r.UserId == stu.UserId);

                if (record == null)
                {
                    record = new TblAttendanceRecord
                    {
                        SessionId = model.SessionId,
                        UserId = stu.UserId
                    };
                    _context.TblAttendanceRecords.Add(record);
                }

                record.StatusId = stu.Status;
                record.Note = stu.Note;
                record.RecordedAt = DateTime.Now;
            }

            _context.SaveChanges();
            return RedirectToAction("Details", new { id = model.classId });
        }

    }
}
