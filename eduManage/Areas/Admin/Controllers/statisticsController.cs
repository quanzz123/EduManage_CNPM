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
        public IActionResult UserActiveByMonth()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetUserActiveByMonth(int year)
        {
            var data = await _context.TblUsers
                .Where(u => (u.IsActive ?? false)                      // chỉ lấy user active
                         && u.CreateDate.HasValue                      // tránh null
                         && u.CreateDate.Value.Year == year)           // truy cập .Value.Year
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
    }

}

