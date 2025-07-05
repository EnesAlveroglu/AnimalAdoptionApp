using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAdoptionApp.Controllers
{
    [Authorize(Roles = "Moderators,Administrators")] // 🔐 Sadece moderatör ve admin erişir
    public class ModeratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}