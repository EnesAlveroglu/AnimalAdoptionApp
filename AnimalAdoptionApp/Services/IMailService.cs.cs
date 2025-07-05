namespace AnimalAdoptionApp.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string body);
    }
}