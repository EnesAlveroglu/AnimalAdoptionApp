using AnimalAdoptionApp.Domain;
using AnimalAdoptionApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimalAdoptionApp.Services;


namespace AnimalAdoptionApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public CommentController(AppDbContext context, UserManager<User> userManager, IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid advertId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                TempData["Error"] = "Yorum boş olamaz.";
                return RedirectToAction("Detail", "Advert", new { id = advertId });
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    TempData["Error"] = "Yorum için giriş yapmalısınız.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var advert = await _context.Adverts
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.Id == advertId);

                if (advert == null)
                {
                    TempData["Error"] = "İlan bulunamadı.";
                    return RedirectToAction("Index", "Advert");
                }

                var comment = new Comment
                {
                    Id = Guid.NewGuid(),
                    Text = text.Trim(),
                    Date = DateTime.UtcNow,
                    AdvertId = advertId,
                    UserId = currentUser.Id
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                // 🔔 Mail gönder: Aynı kullanıcı kendi ilanına yorum yaptıysa gönderme
                if (advert.User?.Email != null && advert.User.Id != currentUser.Id)
                {
                    string subject = "İlanınıza yeni bir yorum yapıldı!";
                    string body = $"<p><strong>{currentUser.UserName}</strong> kullanıcısı <strong>{advert.Title}</strong> ilanınıza yorum yaptı:</p><p>{comment.Text}</p>";

                    await _mailService.SendMailAsync(advert.User.Email, subject, body);
                }
            }
            catch
            {
                TempData["Error"] = "Yorum eklenirken hata oluştu.";
            }

            return RedirectToAction("Detail", "Advert", new { id = advertId });
        }
    }
}
