using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAdoptionApp.Domain;

public class Lineage
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Advert> Adverts { get; set; } = new HashSet<Advert>(); //ilanların türe bağlı listesi //doğrusu HashSete göre sıralamak çok hızlı bulur.
    
}

//Configuration classı = BU entity'i yapılandırmak için kullanıyoruz. Bu ayarların dışında bir ayar yapmak istediğimizde bu classı kullanıyoruz. (nvarchar yerine nchar olsun ya da bu alanı indexle ya da tablonun adı şu olsun gibi extra isteklerimiz varsa onları belirtiyorduk.)

public class LineageConfiguration : IEntityTypeConfiguration<Lineage>  // IEntityTypeConfiguration classını miras aldık(inheritence) bu class Microsoft.EntityFrameworkCore paketinin içinde. aynı zamanda interfacedir classın içinde ne olması gerektiğini söyler. üstüne gelip ctrl + nokta ile implement interface diyoruz ve olması gereken methodu koyuyor. 
{
    public void Configure(EntityTypeBuilder<Lineage> builder)
    {
        builder
       .ToTable("Lineages"); // TPT(Tablo Per Type) = Eğer inherit olmuş type lar varsa her biri için ayr database tablosu oluşturur.(Örnek= Firma tablosu/müşteri tablosu/tedarikçi tablosu miras almışlar diyelim) eğer tablo ismi vermez isek TPH( Table Per Hierarchy=her hiyerarşi için tek tablo ) bir tane tablo yapar firmaları da müşterileri de onun içine koyar tedarikçi tablosunu da aynı tabloya koyar.(TPT kullanmak her zaman iyidir.)Microsoft.EntityFrameworkCore.Relational 



        builder
            .HasIndex(p => new { p.Name }) // Hayvanları a dan z ye ya da z den a ya sıralamak için, arama yaparken arama çubuğundan türleri ararken hızlı olması için index ekliyoruz.
        .IsUnique(false); //bu indexin unique olmasını istiyoruz. yani aynı isimde tür olmasın dersek true aynı isimde türler olabilir dersek false yapıyoruz.


        builder.HasMany(p=>p.Adverts) // bir türün birden fazla ilanı olabilir.
            .WithOne(p=>p.Lineage) // bir ilan bir türe aittir.
            .HasForeignKey(P=>P.LineageId) // lİneageId ile bağlıdır.
            .OnDelete(DeleteBehavior.Restrict); // bağlı kayıt varken ana kaydın silinmesini engellemek için kullanılır.(ana kayıt = Lineage)
        //setnull= silinirse ona bağlı veriler kalır.
        //mvc = çok katmanlı dizaynpeterdır. yani veri katmanıyla database katmanı arasındaki bağımlılığı çok zayıftır. Bu yüzdden postgresql,msssql gibi databaseler kullanılabilir.
    }
}