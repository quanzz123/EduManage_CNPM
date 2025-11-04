using eduManage.Models;
using eduManage.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eduManage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonContentController : Controller
    {
        private readonly EdumanageContext _context;
        private readonly IWebHostEnvironment _env;
        public LessonContentController(EdumanageContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int id)
        {
            var lc = _context.TblLessionContents.Where(l => l.LessionId == id).ToList();
            ViewBag.LessonId = id;
            return View(lc);
        }
        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.LessonId = id;
            return View();
        }
        [HttpPost]
        public IActionResult Create(LessonContentVM? model)
        {
            if (ModelState.IsValid)
            {
                string savedFileUrl = SaveFile(model.FileUpload);
                var lc = new TblLessionContent
                {
                    LessionId = model.LessionId,
                    Title = model.Title,
                    ContentType = model.ContentType,
                    ContentUrl = savedFileUrl,
                    Duration = model.Duration,
                    OrderIdx = model.OrderIdx,
                    CreateDate = model.CreateDate

                };
                _context.TblLessionContents.Add(lc);
                _context.SaveChanges();
                return RedirectToAction("Index", new { id = model.LessionId });
            }
            return View(model);
        }
        public IActionResult Download(int id)
        {
            var lc = _context.TblLessionContents.Find(id);
            if (lc == null || string.IsNullOrEmpty(lc.ContentUrl))
                return NotFound();

            // Dòng quan trọng
            var filePath = Path.Combine(_env.WebRootPath, lc.ContentUrl.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
            {
                TempData["Error"] = "Tệp không tồn tại hoặc đã bị xóa!";
                return RedirectToAction("Index", new { id = lc.ContentId});
            }

            return PhysicalFile(filePath, "application/octet-stream", Path.GetFileName(filePath));
        }
        private string SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Thư mục lưu file trong wwwroot/uploads
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            return "/uploads/" + fileName;
        }
    }
}
