using AnimalAdoptionApp;
using AnimalAdoptionApp.Domain;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args); // içinde bir servis barýndýrýyor configuration diye appsettingsi okuyup içinden veri alabiliyor. (builder.Configuration.GetConnectionString("Npsql")
/*Neler yaptýk
 * databaseye gönerilecek DbContext Classý 
 * rolümüzü tanýmladýk
 * useri tanýmladýk
 * Identity ayarlarýný verdik
 * Authothenticationu kullanýcaðýmýzý söyledik
 * Authorization kullanýcaðýmýzý söyledik
 * Migration ayarlarýný yani databasede bu baðlantýlarý oluþturucak kodlar(Microsoft.EntityFrameworkCore.Tools)
 * Update-Database
 */

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseNpgsql(builder.Configuration.GetConnectionString("Npgsql"));
    //postgresqlserver kullanýcaz paketini indiriyoruz.
    //connectionstring koyucaz. Databaseye nasýl baðlanýcak,Database nerde,hangi kullanýcýyla baðlanýcak onu ifade eden text dosyasý. connectionstrings.com dan alýyoruz. postgresql/Npssql/Standart (postgresql ile baðlýyacaksak)
});

builder.Services.AddIdentity<User, Role>(Config =>
{
    //kullanýcýnýn(user) veri girerken nelere dikkat etsin.(ýdentýty(kimlik) ayarlarý)
    Config.Password.RequireDigit = true; //en az bir rakam mecburi
    Config.Password.RequireLowercase = true; //en az bir küçük harf mecburi
    Config.Password.RequireUppercase = true; //en az bir büyük harf mecburi
    Config.Password.RequireNonAlphanumeric = true; //en az bir sembol rakam mecburi
    Config.Password.RequiredLength = 8; //8 karakterden oluþsun.
    Config.Password.RequiredUniqueChars = 1; //peþ peþe kaç karakter gelebilir
    Config.User.RequireUniqueEmail = true; //Emailler benzersiz olmalý
    //Config.User.AllowedUserNameCharacters = user nameler hangi karakterleri alýr biz usernamelere eposta kullanýcaz gerek yok
    Config.Lockout.MaxFailedAccessAttempts = 5; //5 kere hatalý deneme hakký var
    Config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //5 kere hatalý giriþ yaparsa 5 dakika bnala
    //Config.SignIn.RequireConfirmedPhoneNumber = sms ile doðrulama yapýcaksak (two factor authentication)
    Config.SignIn.RequireConfirmedEmail = true; //eposta doðrulama //þifreyi unutursak e posta ile haberleþmek için.

})
 .AddEntityFrameworkStores<AppDbContext>(); //bu method ile AppDbcontexti tanýtýyoruz. Identity kullanýrken yani üstteki servisi kullanýrken benim databasemi kullan ki kullanýcýlarý oraya kaydet, loginlerini kaydet, Claimlerini oraya kaydet. //metodlarý ezerken kendi classýný yazýp bunu kullan diyoruz. yani diyor ki hangi classý kullanýyým <AppDbContext> verdik. eðer bu kimliklere bir þey eklemek istersek ezip kullanýrýz.

//Authentication = Yani gerçek olup olmadýðýný nasýl anlarýz.kimlik nasýldýr tarif ettik þimdi Authentication  tarif edicez. Gerçek olup olmadýðýný tarif edicez. Sunucu kullanýcý login olduðu zaman bir kuki cripto hazýrlar. Kýrýlmasý imkansýza yakýndýr. Bu kukiyi tekrardan respons içinde  kullanýcýnýn browserýna gönderiyoruz o da kaydediyor bir dahaki seferde bizim kullanýcýmýz mý deðil mi anlýyoruz.Bu bir Authentication yöntemidir.
//Kuki tabanlý bir authentication yapýcaz.

//Authentication(Kimlik Doðrulama) = Kullanýcýnýn kim olduðunu doðrulama süreci (ör: kullanýcý adý + þifre kontrolü).

//Authorization (Yetkilendirme) = Doðrulanan kullanýcýnýn hangi iþlemleri yapmaya yetkisi olduðunu belirleme süreci.

builder.Services.AddAuthentication(); //kullanýcýlarýn kuki criptosu bile olsa ayrý ayrý Authentication etmesi gerekir  burda onlar tanýmlanýyor. Authenticationun defaultu kukidir.




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); //ilk önce kullanýcý olup olmadýðýna bakýcaz. Doðrulicaz.
app.UseAuthorization(); //Sonra yetkilendirme yapýcaz.

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
