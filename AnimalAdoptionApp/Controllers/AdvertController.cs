using AnimalAdoptionApp.Domain;
using AnimalAdoptionApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AnimalAdoptionApp.Controllers
{
    public class AdvertController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public AdvertController(
            AppDbContext context,
            IWebHostEnvironment env,
            UserManager<User> userManager,
            IMailService mailService)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _mailService = mailService;
        }

        public async Task<IActionResult> Index()
        {
            var adverts = await _context.Adverts
            .Include(a => a.User)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

            return View(adverts);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Advert advert)
        {
            try
            {
                if (advert.PhotoFile != null)
                {
                    using var memoryStream = new MemoryStream();
                    await advert.PhotoFile.CopyToAsync(memoryStream);
                    advert.Photo = memoryStream.ToArray();
                }

                if (!ModelState.IsValid)
                {
                    return View(advert);
                }

                advert.Id = Guid.NewGuid();
                advert.UserId = Guid.Parse(_userManager.GetUserId(User)); // Giriş yapan kullanıcı
                advert.Date = DateTime.UtcNow;
                advert.Displays = 0;

                _context.Adverts.Add(advert);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return Content("HATA: " + message);
            }
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var advert = await _context.Adverts
                .Include(a => a.User)
                .Include(a => a.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (advert == null) return NotFound();

            return View(advert);
        }

        // ✅ İlgilen Butonu için Mail Gönderme Aksiyonu
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Interest(Guid id)
        {
            var advert = await _context.Adverts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            var currentUser = await _userManager.GetUserAsync(User);

            if (advert == null || currentUser == null)
            {
                TempData["Error"] = "İlan veya kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            if (advert.UserId == currentUser.Id)
            {
                TempData["Error"] = "Kendi ilanınıza ilgilenemezsiniz.";
                return RedirectToAction("Detail", new { id });
            }

            string subject = $"'{advert.Title}' ilanına bir kullanıcı ilgilendi!";
            string body = $"<p><strong>{currentUser.UserName}</strong> adlı kullanıcı ilanınızla ilgileniyor.</p>";

            await _mailService.SendMailAsync(advert.User.Email, subject, body);

            TempData["Success"] = "İlginiz ilan sahibine iletildi!";
            return RedirectToAction("Detail", new { id });

        }
        [Authorize(Roles = "Moderators,Administrators")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var advert = await _context.Adverts.FindAsync(id);
            if (advert == null)
            {
                return NotFound();
            }

            _context.Adverts.Remove(advert);
            await _context.SaveChangesAsync();

            TempData["Success"] = "İlan silindi.";
            return RedirectToAction("Index");
        }
    }
}



