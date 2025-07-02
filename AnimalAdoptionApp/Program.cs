using AnimalAdoptionApp;
using AnimalAdoptionApp.Domain;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args); // i�inde bir servis bar�nd�r�yor configuration diye appsettingsi okuyup i�inden veri alabiliyor. (builder.Configuration.GetConnectionString("Npsql")
/*Neler yapt�k
 * databaseye g�nerilecek DbContext Class� 
 * rol�m�z� tan�mlad�k
 * useri tan�mlad�k
 * Identity ayarlar�n� verdik
 * Authothenticationu kullan�ca��m�z� s�yledik
 * Authorization kullan�ca��m�z� s�yledik
 * Migration ayarlar�n� yani databasede bu ba�lant�lar� olu�turucak kodlar(Microsoft.EntityFrameworkCore.Tools)
 * Update-Database
 */

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseNpgsql(builder.Configuration.GetConnectionString("Npgsql"));
    //postgresqlserver kullan�caz paketini indiriyoruz.
    //connectionstring koyucaz. Databaseye nas�l ba�lan�cak,Database nerde,hangi kullan�c�yla ba�lan�cak onu ifade eden text dosyas�. connectionstrings.com dan al�yoruz. postgresql/Npssql/Standart (postgresql ile ba�l�yacaksak)
});

builder.Services.AddIdentity<User, Role>(Config =>
{
    //kullan�c�n�n(user) veri girerken nelere dikkat etsin.(�dent�ty(kimlik) ayarlar�)
    Config.Password.RequireDigit = true; //en az bir rakam mecburi
    Config.Password.RequireLowercase = true; //en az bir k���k harf mecburi
    Config.Password.RequireUppercase = true; //en az bir b�y�k harf mecburi
    Config.Password.RequireNonAlphanumeric = true; //en az bir sembol rakam mecburi
    Config.Password.RequiredLength = 8; //8 karakterden olu�sun.
    Config.Password.RequiredUniqueChars = 1; //pe� pe�e ka� karakter gelebilir
    Config.User.RequireUniqueEmail = true; //Emailler benzersiz olmal�
    //Config.User.AllowedUserNameCharacters = user nameler hangi karakterleri al�r biz usernamelere eposta kullan�caz gerek yok
    Config.Lockout.MaxFailedAccessAttempts = 5; //5 kere hatal� deneme hakk� var
    Config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //5 kere hatal� giri� yaparsa 5 dakika bnala
    //Config.SignIn.RequireConfirmedPhoneNumber = sms ile do�rulama yap�caksak (two factor authentication)
    Config.SignIn.RequireConfirmedEmail = true; //eposta do�rulama //�ifreyi unutursak e posta ile haberle�mek i�in.

})
 .AddEntityFrameworkStores<AppDbContext>(); //bu method ile AppDbcontexti tan�t�yoruz. Identity kullan�rken yani �stteki servisi kullan�rken benim databasemi kullan ki kullan�c�lar� oraya kaydet, loginlerini kaydet, Claimlerini oraya kaydet. //metodlar� ezerken kendi class�n� yaz�p bunu kullan diyoruz. yani diyor ki hangi class� kullan�y�m <AppDbContext> verdik. e�er bu kimliklere bir �ey eklemek istersek ezip kullan�r�z.

//Authentication = Yani ger�ek olup olmad���n� nas�l anlar�z.kimlik nas�ld�r tarif ettik �imdi Authentication  tarif edicez. Ger�ek olup olmad���n� tarif edicez. Sunucu kullan�c� login oldu�u zaman bir kuki cripto haz�rlar. K�r�lmas� imkans�za yak�nd�r. Bu kukiyi tekrardan respons i�inde  kullan�c�n�n browser�na g�nderiyoruz o da kaydediyor bir dahaki seferde bizim kullan�c�m�z m� de�il mi anl�yoruz.Bu bir Authentication y�ntemidir.
//Kuki tabanl� bir authentication yap�caz.

//Authentication(Kimlik Do�rulama) = Kullan�c�n�n kim oldu�unu do�rulama s�reci (�r: kullan�c� ad� + �ifre kontrol�).

//Authorization (Yetkilendirme) = Do�rulanan kullan�c�n�n hangi i�lemleri yapmaya yetkisi oldu�unu belirleme s�reci.

builder.Services.AddAuthentication(); //kullan�c�lar�n kuki criptosu bile olsa ayr� ayr� Authentication etmesi gerekir  burda onlar tan�mlan�yor. Authenticationun defaultu kukidir.




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

app.UseAuthentication(); //ilk �nce kullan�c� olup olmad���na bak�caz. Do�rulicaz.
app.UseAuthorization(); //Sonra yetkilendirme yap�caz.

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
