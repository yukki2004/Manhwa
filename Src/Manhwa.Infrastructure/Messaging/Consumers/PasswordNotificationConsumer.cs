using Manhwa.Application.Common.Messaging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Messaging.Consumers
{
    public class PasswordNotificationConsumer : IConsumer<PasswordChangedNotificationEvent>
    {
        private readonly IConfiguration _configuration;

        public PasswordNotificationConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<PasswordChangedNotificationEvent> context)
        {
            var eventData = context.Message;
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var senderName = _configuration["EmailSettings:SenderName"] ?? "TruyenVerse Security";
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:Password"];

            string htmlBody = GenerateSecurityAlertHtml(eventData.Username, eventData.IpAddress, eventData.UserAgent);

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = "Cảnh báo bảo mật: Mật khẩu của bạn đã thay đổi - TruyenVerse",
                Body = htmlBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(eventData.Email);

            await client.SendMailAsync(mailMessage);
        }

        private string GenerateSecurityAlertHtml(string username, string ip, string userAgent)
        {
            var html = new StringBuilder();
            html.Append($@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <style>
                    body {{ font-family: 'Inter', sans-serif; background-color: #0f172a; margin: 0; padding: 0; }}
                    .wrapper {{ background-color: #0f172a; padding: 40px 20px; }}
                    .container {{ 
                        background-color: #1e293b; 
                        border-radius: 16px; 
                        max-width: 550px; 
                        margin: 0 auto; 
                        padding: 40px; 
                        border: 1px solid #334155;
                    }}
                    .logo {{ font-size: 26px; font-weight: 800; color: #6366f1; text-align: center; margin-bottom: 24px; }}
                    .title {{ color: #f8fafc; font-size: 20px; font-weight: 600; text-align: center; margin-bottom: 20px; }}
                    .content {{ color: #94a3b8; font-size: 15px; line-height: 1.6; }}
                    .info-box {{ 
                        background-color: #0f172a; 
                        padding: 15px; 
                        border-radius: 10px; 
                        margin: 20px 0;
                        border-left: 4px solid #f43f5e;
                    }}
                    .info-item {{ color: #cbd5e1; font-size: 14px; margin: 5px 0; }}
                    .info-item b {{ color: #6366f1; }}
                    .alert {{ color: #fb7185; font-size: 14px; font-weight: 500; margin-top: 20px; }}
                    .footer {{ color: #64748b; font-size: 12px; text-align: center; border-top: 1px solid #334155; padding-top: 20px; margin-top: 30px; }}
                </style>
            </head>
            <body>
                <div class='wrapper'>
                    <div class='container'>
                        <div class='logo'>TruyenVerse</div>
                        <div class='title'>Thông báo thay đổi mật khẩu</div>
                        <div class='content'>
                            Chào <b>{username}</b>,<br>
                            Chúng tôi nhận thấy mật khẩu tài khoản TruyenVerse của bạn vừa được thay đổi thành công.
                        </div>
                        <div class='info-box'>
                            <div class='info-item'><b>Thời gian:</b> {DateTimeOffset.UtcNow:dd/MM/yyyy HH:mm:ss} (UTC)</div>
                            <div class='info-item'><b>Địa chỉ IP:</b> {ip}</div>
                            <div class='info-item'><b>Thiết bị:</b> {userAgent}</div>
                        </div>
                        <div class='alert'>
                            Nếu bạn KHÔNG thực hiện thay đổi này, tài khoản của bạn có thể đang gặp nguy hiểm. Hãy sử dụng chức năng quên mật khẩu ngay lập tức để lấy lại quyền truy cập.
                        </div>
                        <div class='footer'>
                            © {DateTime.Now.Year} TruyenVerse Team. Đây là email tự động, vui lòng không trả lời.
                        </div>
                    </div>
                </div>
            </body>
            </html>");
            return html.ToString();
        }
    }
}

