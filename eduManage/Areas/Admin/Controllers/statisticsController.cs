using eduManage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class statisticsController : Controller
    {
        private readonly EdumanageContext _context;
        public statisticsController(EdumanageContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserActiveByMonth()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetUserActiveByMonth(int year)
        {
            var data = await _context.TblUsers
                .Where(u => (u.IsActive ?? false)                      
                         && u.CreateDate.HasValue                      
                         && u.CreateDate.Value.Year == year)           
                .GroupBy(u => u.CreateDate.Value.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    SoLuong = g.Count()
                })
                .OrderBy(x => x.Thang)
                .ToListAsync();

            // Đảm bảo có đủ 12 tháng, gán 0 nếu tháng chưa có user
            var fullData = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    Thang = m,
                    SoLuong = data.FirstOrDefault(x => x.Thang == m)?.SoLuong ?? 0
                });

            return Json(fullData);
        }

        public IActionResult StudentByClass()

        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetStudentByClass()
        {
            var data = await _context.TblClasses
                .GroupJoin(_context.TblClassMembers,
                c => c.ClassId, cm => cm.ClassId,
                (c, members) => new
                {
                    TenLop = c.ClassName,
                    Soluong = members.Count()
                }).ToListAsync();
            return Json(data);
        }

        public IActionResult SubmissionRate()
        {

            return View();
        }
        public async Task<IActionResult> GetSubmissionRate(int classid)
        {
            var submission = await (from s in _context.Submissions
                                    join a in _context.Assignments on s.AssignmentId equals a.AssignmentId
                                    join c in _context.TblClasses on a.ClassId equals c.ClassId
                                    where a.ClassId == classid
                                    select s.SubmissionId).Distinct().ToListAsync();
            var classes = await (from c in _context.TblClasses
                                 join cm in _context.TblClassMembers on c.ClassId equals cm.ClassId
                                 where c.ClassId == classid
                                 select cm.UserId).Distinct().ToListAsync();
            int totalSubmissions = submission.Count();
            int totalStudents = classes.Count();
            int notSubmissions = totalStudents - totalSubmissions;

            var result = new
            {
                Danop = totalSubmissions,
                Tong = totalStudents,
                chuanop = notSubmissions
            };
            Console.WriteLine(classid);
            Console.WriteLine(result.Danop);
            Console.WriteLine(result.Tong);
            Console.WriteLine(result.chuanop);
            return Json(result);
        }
        [HttpGet]
        public IActionResult getClasses()
        {
            var classes = _context.TblClasses.Select(c => new
            {
                classId = c.ClassId,
                className = c.ClassName
            }).ToList();
            return Json(classes);
        }
    }
}

