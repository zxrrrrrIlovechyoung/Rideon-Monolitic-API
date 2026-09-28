using System.Net;
using System.Net.Mail;

public interface IEmailService
{
    void Send(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void Send(string to, string subject, string body)
    {
        var smtpSection = _config.GetSection("Smtp");
        var host = smtpSection["Host"];
        var port = int.Parse(smtpSection["Port"]);
        var user = smtpSection["User"];
        var password = smtpSection["Password"];

        using (var client = new SmtpClient(host, port))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(user, password);

            var mail = new MailMessage(user, to, subject, body);
            mail.IsBodyHtml = true;

            client.Send(mail);
        }
    }
}
