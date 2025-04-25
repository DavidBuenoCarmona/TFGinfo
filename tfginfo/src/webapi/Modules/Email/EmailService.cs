using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string body, bool isHtml = true)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        email.To.Add(new MailboxAddress("", recipientEmail));
        email.Subject = subject;

        // Configurar el cuerpo del correo
        email.Body = new TextPart(isHtml ? "html" : "plain")
        {
            Text = body
        };
        Console.WriteLine($"Sending email to {recipientEmail} with subject: {subject}");

        using var smtp = new SmtpClient();
        try
        {
            
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            Console.WriteLine($"Connecting to SMTP server {_emailSettings.SmtpServer} on port {_emailSettings.SmtpPort} with email {_emailSettings.SenderEmail} and password {_emailSettings.SenderPassword}");
            await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw; 
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}