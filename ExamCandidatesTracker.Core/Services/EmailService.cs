using Exam.Core.DTOs;
using Exam.Core.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net;

namespace Exam.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("EmailNotification");
        }
        /// <summary>
        /// The method sends Email to Candidates with Attached QR Code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> SendEmail(EmailDto emailDetails)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_configuration.GetSection("UserName").Value);
                email.To.Add(MailboxAddress.Parse(emailDetails.ToEmail));
                email.Subject = emailDetails.Subject;
                var builder = new BodyBuilder();

                using (var wc = new WebClient())
                {

                    builder.Attachments.Add(Path.GetFileName(emailDetails.FilePath),
                    wc.DownloadData(emailDetails.FilePath));

                }

                builder.HtmlBody = emailDetails.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_configuration.GetSection("Host").Value, 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_configuration.GetSection("UserName").Value, _configuration.GetSection("Password").Value);
                await smtp.SendAsync(email);

                File.Delete(emailDetails.FilePath);
            }
            catch
            {
                return ResponseDto<bool>.Fail("Failed Sending Email", (int)HttpStatusCode.Forbidden);
            }

            return ResponseDto<bool>.Success("Successfully sent email", true, (int)HttpStatusCode.OK);
        }
    }
}
