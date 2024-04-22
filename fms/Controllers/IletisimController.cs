using Microsoft.AspNetCore.Mvc;

namespace fms.Controllers
{
    using fms.Models;
    using Microsoft.AspNetCore.Mvc;

    public class IletisimController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IletisimFormuModel model)
        {
            if (ModelState.IsValid)
            {
             

                TempData["Mesaj"] = "İletişim formunuz başarıyla gönderildi!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
