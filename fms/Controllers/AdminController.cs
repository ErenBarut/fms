using AspNetCoreHero.ToastNotification.Abstractions;
using fms.Models;
using fms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fms.Controllers
{

    public class AdminController : Controller
    {
        private static List<FileViewModel> _fileList = new List<FileViewModel>();
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly INotyfService _notify;
        private readonly AppDbContext _context;


        public AdminController(ILogger<AdminController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, INotyfService notify, AppDbContext context)
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
            return View();
        }
        public async Task<IActionResult> GetUserList()
        {
            var userModels = await _userManager.Users.Select(x => new UserModel()
            {

                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                UserName = x.UserName,
            }).ToListAsync();
            return View(userModels);
        }




        [HttpPost]
        public IActionResult DeleteUser(string userId)
        {
            var deleteToUser = _context.Users.Find(userId);
            _context.Users.Remove(deleteToUser);
            _context.SaveChanges(); 
            return Json(new { success = true, message = "Kullanıcı başarıyla silindi." });
        }
    }

}


