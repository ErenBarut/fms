using fms.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using fms.Models;
using fms.ViewModels;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;

namespace fms.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;


        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            
            
            
            






            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz Kullanıcı Adı veya Parola!");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

            if (signInResult.Succeeded)
            {
                if (model.UserName == "admin")
                {

                    return RedirectToAction("Index","Admin");
                }
                return RedirectToAction("Index");
            }
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Kullanıcı Girişi " + user.LockoutEnd + " kadar kısıtlanmıştır!");
                return View();
            }
            ModelState.AddModelError("", "Geçersiz Kullanıcı Adı veya Parola Başarısız Giriş Sayısı :" + await _userManager.GetAccessFailedCountAsync(user) + "/3");
            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);

            }
            var identityResult = await _userManager.CreateAsync(new() { UserName = model.UserName, Email = model.Email, FullName = model.FullName }, model.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            var roleExist = await _roleManager.RoleExistsAsync("Uye");
            if (!roleExist)
            {
                var role = new AppRole { Name = "Uye" };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, "Uye");

            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> GetRoleList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult RoleAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleAdd(AppRole model)
        {
            var role = await _roleManager.FindByNameAsync(model.Name);
            if (role == null)
            {

                var newrole = new AppRole();
                newrole.Name = model.Name; ;
                await _roleManager.CreateAsync(newrole);
            }
            return RedirectToAction("GetRoleList");
        }
        [Authorize]
        public async Task<IActionResult> UserPage()
        {
            var id = _userManager.GetUserId(User);
            var userRole = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(id));

            var userModel = await _context.Users
                .Where(x => x.Id == id)
                .Select(x => new UserModel()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    UserName = x.UserName,
                    Role = userRole.FirstOrDefault() // userRole bir liste, bu nedenle ilk elemanı alıyoruz
                })
                .SingleOrDefaultAsync(); // SingleOrDefault'ı async olarak kullanmak için bu metodu kullanmalısınız

            if (userModel != null)
            {
                return View(userModel);
            }
            else
            {
                // Kullanıcı bulunamadı
                return NotFound();
            }
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}