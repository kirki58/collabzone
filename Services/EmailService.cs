using System.Net;
using System.Net.Mail;
using System.Text;
using collabzone.Settings;
using Microsoft.Extensions.Options;

namespace collabzone.Services;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }
    public async Task SendValidationMail(string toEmail, Guid token){
        //Create the message
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(_emailSettings.SenderEmail);
        mail.To.Add(toEmail);
        mail.Subject = "Email Verification";
        mail.IsBodyHtml = true;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("<h1>Time to start collaborating!</h1>");
        sb.AppendFormat("<p>Click the link below to verify your email address:</p>");
        sb.AppendFormat($"<a href='https://localhost:7217/api/users/validate-email/{token.ToString()}'>Verify Email</a>");

        mail.Body = sb.ToString();

        //send the message
        using(SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)){
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            await client.SendMailAsync(mail);
        };
    }
}
