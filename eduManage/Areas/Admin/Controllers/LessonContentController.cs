using eduManage.Models;
using eduManage.Utilities;
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
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
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

        [HttpGet]
        public IActionResult Edit(int contentid)
        {
            var lessonContent = _context.TblLessionContents.FirstOrDefault(lc => lc.ContentId == contentid);
            if (lessonContent == null)
            {
                Console.WriteLine("Khong tim thay bai giang");
                return NotFound();
            }
            Console.WriteLine("Tieu de"+ lessonContent.Title);
            LessonContentVM vm = new LessonContentVM
            {
                ContentId = lessonContent.ContentId,
                LessionId = lessonContent.LessionId,
                Title = lessonContent.Title,
                ContentType = lessonContent.ContentType,
                ContentUrl = lessonContent.ContentUrl,
                Duration = lessonContent.Duration,
                OrderIdx = lessonContent.OrderIdx,
                CreateDate = lessonContent.CreateDate
            };
            ViewBag.LessonId = lessonContent.LessionId;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(LessonContentVM vm, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lessonContent = _context.TblLessionContents.FirstOrDefault(lc => lc.ContentId == vm.ContentId);
                    if (lessonContent == null)
                    {
                        return NotFound();
                    }
                    if(vm.Duration <= 0)
                    {
                        ModelState.AddModelError("Duration", "Thời lượng phải lớn hơn 0.");
                        ViewBag.error = "Thời lượng phải lớn hơn 0.";
                        return View(vm);
                    }
                    lessonContent.Title = vm.Title;
                    lessonContent.ContentType = vm.ContentType;
                    lessonContent.Duration = vm.Duration;
                    lessonContent.OrderIdx = vm.OrderIdx;
                    if (file != null && file.Length > 0)
                    {
                        // Xóa file cũ nếu có
                        if (!string.IsNullOrEmpty(lessonContent.ContentUrl))
                        {
                            var oldFilePath = Path.Combine(_env.WebRootPath, lessonContent.ContentUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                        // Lưu file mới
                        var savedFileUrl = SaveFile(file);
                        lessonContent.ContentUrl = savedFileUrl;
                    }
                    _context.TblLessionContents.Update(lessonContent);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = vm.LessionId });
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật bài giảng.");
                return View();
            }

            return View(vm);
        }
    }
}
