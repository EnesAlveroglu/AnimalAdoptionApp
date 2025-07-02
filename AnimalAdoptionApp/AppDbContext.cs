using AnimalAdoptionApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace AnimalAdoptionApp;

public class AppDbContext(DbContextOptions options) : IdentityDbContext< //bütün veritabanı işlemleri için kullanılır nugetten paket: microsoft.entity.framework.core // DbContextOptions options Hangi veritabanı sağlayıcısını (SQL Server, PostgreSQL, SQLite vs.) kullanacağını Bağlantı cümlesi (ConnectionString) gibi DbContext ayarlarını belirler.


    User, 
    Role, 
    Guid,
    IdentityUserClaim<Guid>, //Kullanıcıya ait bilgiler (ad, email, rol gibi) token içinde saklanır (claim(bilgi) olarak). 
    //Authentication: Kullanıcı kimliği ve özelliklerini doğrulama ve taşımada kullanılır.
    IdentityUserRole<Guid>, //	Kullanıcı ile roller arasındaki ilişkiyi tutar (çoktan çoka ilişki). ve böyle bir tablo oluşturur. ilave bir şey eklemek istersek miras alıyoruz. onu kullanıyoruz. burdan hangisini kullanmak istersek onu ezip ya da kendi classlarımızı koyup kullanabiliyoruz.Authorization: Kullanıcının hangi rollerde olduğunu tutar. Rollere göre yetki verilir.
    IdentityUserLogin<Guid>, //Harici oturum açma sağlayıcıları (Google, Facebook) ile ilişkili bilgileri tutar. bir şey ilave etmek istersek miras alıp ezebiliriz. Authentication: Kullanıcının kimliğini harici servis üzerinden doğrulama ile ilgilidir.
    IdentityRoleClaim<Guid>, //Role ait claim bilgilerini tutar (örneğin rolün izinleri). oturum açan userın hangi rolde olup olmadığını tutar.Authorization: Rolün hangi izinlere sahip olduğunu belirler.
    IdentityUserToken<Guid>>(options) //"Kullanıcıya ait özel token bilgilerini tutar (örneğin refresh token, 2FA tokenları, harici tokenlar)"Refresh Token:
/*
Kullanıcının oturum süresi dolduğunda, tekrar giriş yapmadan yeni erişim tokenı (access token) almak için kullanılır.

Böylece kullanıcı sürekli şifre girmek zorunda kalmaz.

2FA Tokenları (İki Faktörlü Doğrulama):

İki adımlı doğrulama yaparken kullanıcıya gönderilen veya kullanılan geçici kodlar(SMS, e-posta veya uygulama üzerinden).

Bu tokenlar sistemde geçicidir ve ekstra güvenlik sağlar.

Harici Tokenlar:

Kullanıcı Google, Facebook gibi harici servislerle giriş yaptıysa, o servisin sağladığı token bilgileri burada tutulabilir.

Örneğin, Facebook oturumu için verilen erişim tokenı.
*/

{
    protected override void OnModelCreating(ModelBuilder builder) //modeli databasede oluştururken bu methodu kullan. ve configurationları çağırttırıyoruz aşağıdaki method ile.
    {
        base.OnModelCreating(builder); 
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); //configurationları çağırttırıyoruz bu method ile.
    }
     
    //Userın DbSeti ve Rolün DbSeti zaten atasından geliyor onları koymamıza gerek yok.
    public required DbSet<Lineage> Lineages { get; set; }
    public required DbSet<Advert> Adverts { get; set; }
    public required DbSet<Comment> Comments { get; set; }
}
