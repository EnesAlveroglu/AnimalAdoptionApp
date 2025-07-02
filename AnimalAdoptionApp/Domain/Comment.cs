using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography.X509Certificates;

namespace AnimalAdoptionApp.Domain;

public class Comment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } 
    public Guid AdvertId { get; set; }
    public required string Text { get; set; }
    public DateTime Date { get; set; }
    public int Likes { get; set; }
    public User? User { get; set; }
    public Advert? Advert { get; set; }

}
public class commentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .ToTable("comments"); // tablo ismi comments olsun.

        builder
            .HasIndex(p => new { p.Date }) // yorumları tarihe göre sıralamak için index ekliyoruz.
            .IsUnique(false); // bu indexin unique olmasını istiyoruz. yani aynı tarihte birden fazla yorum olabilir dersek false yapıyoruz.
        //default olarak ascending sıralama yönü küçükten büyüğe doğrudur.
            

    }
}
