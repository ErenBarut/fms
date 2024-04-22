// FileUploadController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using fms.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.IO;
using Microsoft.EntityFrameworkCore;
using fms.Models;
using Microsoft.AspNetCore.Identity;

namespace fms.Controllers
{
    public class FileUploadController : Controller.
    {
        private static List<FileViewModel> _fileList = new List<FileViewModel>();
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly INotyfService _notify;
        private readonly AppDbContext _context;

        public FileUploadController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager,INotyfService notify, AppDbContext context )
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _notify = notify; 
            _context = context;
            
        }

        public IActionResult Index()
        {
            var fileListModel = new FileViewModel
            {
                Files = _fileList
            };

            return View(fileListModel);
        }
        public IActionResult MyFiles()
        {
            var UserID = _userManager.GetUserId(User);
            var DosyaModel = _context.Dosyalar.Where(s => s.UserId == UserID)
                    .Select(x => new
                    DosyaModel
                    {
                        
                        DosyaId = x.DosyaId,
                        DosyaName = x.DosyaName,
                        DosyaDescription = x.DosyaDescription,
                        DosyaPath = x.DosyaPath,
                        DosyaTime = x.DosyaTime


    }).ToList();

            return View(DosyaModel);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(FileViewModel model)
        {
            if (model.File != null && model.File.Length > 0)
            {
                var fileName = model.File.FileName;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }

                var fileViewModel = new FileViewModel
                {
                    FileName = fileName,
                    Description = model.Description
                };

                _fileList.Add(fileViewModel);
                try
                {
                    Dosyalar newDosya = new()
                    {
                        UserId = _userManager.GetUserId(User),
                        DosyaName = fileName,
                        DosyaPath = filePath,
                        DosyaDescription = model.Description,
                        DosyaTime = DateTime.Now,
                    };
                    _context.Dosyalar.Add(newDosya);

                    _context.SaveChanges();
                    _notify.Success("Veritabanına veriler eklendi");
                }
                catch
                {
                    _notify.Error("Veri tabanına eklerken sorun oluştu");
                }

                _notify.Success("Dosya yükleme başarılı!");
            }
            else
            {
                _notify.Error("Dosya seçilmedi veya dosya boş!");
            }
           

            var fileListModel = new FileViewModel
            {
                Files = _fileList
            };

            return View("Index", fileListModel);
        }

        public IActionResult Download(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            if (System.IO.File.Exists(filePath))
            {
                var fileContent = System.IO.File.ReadAllBytes(filePath);
                return File(fileContent, "application/octet-stream", fileName);
            }

            return NotFound();
        }

        public IActionResult Delete(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                _fileList.RemoveAll(f => f.FileName == fileName);

                _notify.Success($"{fileName} dosyası başarıyla silindi.");
            }
            else
            {
                _notify.Error($"{fileName} dosyası bulunamadığı için silme işlemi gerçekleştirilemedi.");
            }

            var fileListModel = new FileViewModel
            {
                Files = _fileList
            };

            return View("Index", fileListModel);
        }
    }
}
