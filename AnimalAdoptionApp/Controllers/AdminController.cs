using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAdoptionApp.Controllers
{
    [Authorize(Roles = "Administrators")] // 🔒 Sadece admin erişebilir
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Views/Admin/Index.cshtml sayfası
        }
    }
}

