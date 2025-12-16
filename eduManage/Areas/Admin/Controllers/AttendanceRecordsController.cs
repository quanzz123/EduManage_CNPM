using eduManage.Models;
using eduManage.Utilities;
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
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            var userId = Functions._UserId;
            var claslist = _context.TblClasses.Where(c => c.TeacherId == userId).ToList();
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
            var existingRecords2 = (from r in _context.TblAttendanceRecords
                                   join s in _context.TblAttendanceSessions on r.SessionId equals s.SessionsId
                                   join c in _context.TblClasses on s.ClassId equals c.ClassId
                                   join m in _context.TblClassMembers on c.ClassId equals m.ClassId
                                   where r.SessionId == id
                                   select new
                                   {
                                       Id = r.UserId,
                                       sts = r.Status,
                                       note = r.Note,
                                       namcode = m.Note,
                                       fullname = r.User.FullName


                                   }).ToList();
            var existingRecords3 = _context.TblAttendanceRecords
            .Where(r => r.SessionId == id)
            .Select(r => new
            {
                Id = r.UserId,
                sts = r.Status,
                note = r.Note,
                namcode = r.Session.Class.TblClassMembers
                              .FirstOrDefault(m => m.UserId == r.UserId).Note,
                fullname = r.User.FullName
            })
            .ToList();
            List<AttendanceItem> students;
            if (existingRecords3.Any())
            {
                students = existingRecords
                    .Select(r => new AttendanceItem
                    {
                        UserId = r.UserId,
                        FullName = r.User.FullName,
                        Status = r.StatusId,
                        Note = r.Note,
                        NameCode = existingRecords3
                            .FirstOrDefault(e => e.Id == r.UserId)?.namcode

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
                        Status = 1 ,
                        NameCode = x.Note 
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
