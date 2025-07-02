using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAdoptionApp.Domain;

public class Role : IdentityRole<Guid> //(Rol üyelik sistemi) iki tane entity'si(varlık) var bunlardan birisi Role üyelik sistemi bunla ile ilgili her şeyi IdentityRole classı yapıyor(password,email.tel vb.). Ondan miras alıyoruz.IdentityRole classını kullanabilmek için Microsoft.AspNetCore.Identity paketini nugetten indiriyoruz.Guid(benzersiz ıd) kullanıcağımız için <Guid> şeklinde yanına ekledik. yeni bir action tanmladığımızda şu roldeki adamlar bu actionu çalıştırabilir diyicez.(rolü masa gibi düşünün.) Masa başında oturanlar bu roldeki kişiler. Bu kişiler o masanın yetkilerini kullanır. bir kişi birden fazla rolde olabilir. bir rolde birden fazla kişi olabilir.

{
   
}
