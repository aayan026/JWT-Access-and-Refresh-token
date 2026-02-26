using EShop.Application.Services.Abstracts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Infrastructure.Services.Concretes;

public class EmailService : IEmailService
{

    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }
    public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
    {
        var smptClient = new SmtpClient();

        smptClient.Host = _emailSettings.Host;
        smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smptClient.UseDefaultCredentials = false;
        smptClient.Port = 587;
        smptClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
        smptClient.EnableSsl = true;

        var mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(_emailSettings.Email);
        mailMessage.To.Add(ToEmail);

        mailMessage.Body = @$"
    <h4>Şifrənizi yeniləmək üçün aşağıdakı token-i kopyalayın:</h4>
    <p><b>Token:</b> {resetPasswordEmailLink}</p>
    <p>Swagger-də ResetPassword endpoint-inə bu token-i daxil edin.</p>";
        mailMessage.IsBodyHtml = true;


        await smptClient.SendMailAsync(mailMessage);
    }
    
    public async Task SendConfirmEmail(string token, string toEmail)
    {
        var smtpClient = new SmtpClient();
        smtpClient.Host = _emailSettings.Host;
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Port = 587;
        smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
        smtpClient.EnableSsl = true;

        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_emailSettings.Email);
        mailMessage.To.Add(toEmail);
        mailMessage.Subject = "Email Təsdiqləmə";
        mailMessage.Body = @$"
        <h4>Hesabınızı təsdiqləmək üçün aşağıdakı token-i kopyalayın:</h4>
        <p><b>Token:</b> {token}</p>
        <p>Swagger-də ConfirmEmail endpoint-inə bu token-i daxil edin.</p>";
        mailMessage.IsBodyHtml = true;

        await smtpClient.SendMailAsync(mailMessage);
    }

}
