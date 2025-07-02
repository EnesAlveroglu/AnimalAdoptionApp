using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;


namespace AnimalAdoptionApp.Domain;

public enum Gender
{
    Erkek,
    Disi
}
public class Advert
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid LineageId { get; set; }
    public string? Description { get; set; }
    public required byte[] Photo { get; set; }
    public DateTime Date { get; set; }
    public decimal Kg { get; set; }
    public required string Age { get; set; }
    public required Gender Gender { get; set; }
    public User? User { get; set; }
    public Lineage? Lineage { get; set; }

    [NotMapped]
    public IFormFile? PhotoFile { get; set; }
    public int Displays { get; set; } //ilana tıklandığı zaman bir arttırılır.
    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>(); //ilanın yorumları

}

public class AdvertConfiguration : IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
    {
        builder
            .ToTable("Adverts");
        builder
            .HasIndex(p => new { p.Date })
            .IsUnique(false)
            .IsDescending(true); // tarihleri büyükten küçüğe doğru sıralar.Databaseden çekişi hızlandırır.(önemli!!!)
        builder
             .HasMany(p => p.Comments) // bir türün birden fazla ilanı olabilir.
            .WithOne(p => p.Advert) // bir ilan bir türe aittir.
            .HasForeignKey(P => P.AdvertId) // AdvertId ile bağlıdır.
            .OnDelete(DeleteBehavior.Restrict); // Advert silinemesin çünkü bağlı olduğu kayıt var.(ana kayıt = Advert)
        //setnull= silinirse ona bağlı veriler kalır.
    }
}