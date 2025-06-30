using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration,ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");

        using (var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
        {
            client.Credentials = new NetworkCredential(smtpSettings["User"], smtpSettings["Password"]);
            client.EnableSsl = bool.Parse(smtpSettings["EnableSSL"]); // Ensure SSL is enabled for secure connections

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["User"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await client.SendMailAsync(mailMessage);
                _logger.LogDebug("Mail Send Successfully");
            }
            catch (Exception ex)
            {
                // Log the detailed exception message
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
        }
    }
}
