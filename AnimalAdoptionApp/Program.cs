using AnimalAdoptionApp;
using AnimalAdoptionApp.Domain;
using AnimalAdoptionApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısı
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Npgsql")));

// Identity servisleri
builder.Services.AddIdentity<User, Role>(options => {
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Mail servisleri
builder.Services.AddTransient<IMailService, MailService>(); // Bizim özel mail servisimiz
builder.Services.AddTransient<IEmailSender, MailService>(); // Identity sistemine entegre

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Geliştirme ortamı
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Rota tanımı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


// ✅ Rol ve Admin Kullanıcısını Otomatik Oluştur
using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    string[] roles = { "Administrators", "Moderators", "Members" };

    foreach (var roleName in roles)
    {
        if (!await roleMgr.RoleExistsAsync(roleName))
            await roleMgr.CreateAsync(new Role { Name = roleName });
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var adminEmail = "admin@example.com"; // Bu email ile kayıt olursan admin yapılır
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Administrators"))
    {
        await userManager.AddToRoleAsync(adminUser, "Administrators");
    }
}
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var moderatorEmail = "mod@example.com";
    var moderatorUser = await userManager.FindByEmailAsync(moderatorEmail);

    if (moderatorUser != null && !await userManager.IsInRoleAsync(moderatorUser, "Moderators"))
    {
        await userManager.AddToRoleAsync(moderatorUser, "Moderators");
    }
}


app.Run();
