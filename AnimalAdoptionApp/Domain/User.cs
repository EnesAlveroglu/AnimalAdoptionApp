using Microsoft.AspNetCore.Identity;

namespace AnimalAdoptionApp.Domain
{
    public class User : IdentityUser<Guid> // IdentityUser(Kullanıcı üyelik sistemi) classını microsoft.AspNetCore.Identity paketinden alıyoruz. Guid(benzersiz ıd) kullanıcağımız için <Guid> şeklinde yanına ekledik. IdentityUser içinde (username, password, email, phone number gibi) microsoft ad soyad koymamış ama bizdeki her kişi bir kullanıcı olduğu için ad soyad ekliyoruz.
    {
        public required string GivenName { get; set; }
        public DateTime Date { get; set; }

    }
}
