using Manhwa.Application.Common.Messaging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Messaging.Consumers
{
    public class SendOtpEmailConsumer : IConsumer<SendOtpEmailEvent>
    {
        private readonly IConfiguration _configuration;

        public SendOtpEmailConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<SendOtpEmailEvent> context)
        {
            var eventData = context.Message;

            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var senderName = _configuration["EmailSettings:SenderName"] ?? "TruyenVerse";
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:Password"];

            string htmlBody = GenerateOtpEmailHtml(eventData.OtpCode);

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = $"[{eventData.OtpCode}] Mã xác nhận đặt lại mật khẩu - TruyenVerse",
                Body = htmlBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(eventData.Email);

            await client.SendMailAsync(mailMessage);
        }

        private string GenerateOtpEmailHtml(string otpCode)
        {
            var html = new StringBuilder();
            html.Append($@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <style>
                    body {{ font-family: 'Inter', 'Segoe UI', Arial, sans-serif; background-color: #0f172a; margin: 0; padding: 0; }}
                    .wrapper {{ background-color: #0f172a; padding: 40px 20px; }}
                    .container {{ 
                        background-color: #1e293b; 
                        border-radius: 16px; 
                        max-width: 500px; 
                        margin: 0 auto; 
                        padding: 40px; 
                        text-align: center;
                        box-shadow: 0 10px 25px rgba(0,0,0,0.3);
                        border: 1px solid #334155;
                    }}
                    .logo {{ 
                        font-size: 28px; 
                        font-weight: 800; 
                        color: #6366f1; 
                        margin-bottom: 24px; 
                        text-transform: uppercase;
                        letter-spacing: 2px;
                    }}
                    .title {{ color: #f8fafc; font-size: 22px; font-weight: 600; margin-bottom: 16px; }}
                    .content {{ color: #94a3b8; font-size: 16px; line-height: 1.6; margin-bottom: 32px; }}
                    .otp-box {{ 
                        background: linear-gradient(135deg, #6366f1 0%, #a855f7 100%);
                        color: #ffffff; 
                        font-size: 38px; 
                        font-weight: 700; 
                        letter-spacing: 10px; 
                        padding: 20px; 
                        border-radius: 12px; 
                        display: inline-block;
                        margin: 0 auto 32px auto;
                        box-shadow: 0 4px 15px rgba(99, 102, 241, 0.4);
                    }}
                    .warning {{ 
                        background-color: #334155; 
                        color: #cbd5e1; 
                        padding: 12px; 
                        border-radius: 8px; 
                        font-size: 13px; 
                        margin-bottom: 24px;
                    }}
                    .footer {{ color: #64748b; font-size: 12px; border-top: 1px solid #334155; padding-top: 24px; }}
                    .footer b {{ color: #94a3b8; }}
                </style>
            </head>
            <body>
                <div class='wrapper'>
                    <div class='container'>
                        <div class='logo'>TruyenVerse</div>
                        <div class='title'>Xác thực yêu cầu của bạn</div>
                        <div class='content'>
                            Chào bạn,<br>
                            Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản TruyenVerse. 
                            Vui lòng sử dụng mã OTP bên dưới để tiếp tục:
                        </div>
                        <div class='otp-box'>{otpCode}</div>
                        <div class='warning'>
                            Mã này sẽ hết hạn sau <b>5 phút</b>.<br>
                            Nếu bạn không yêu cầu mã này, hãy bỏ qua email này để bảo vệ tài khoản.
                        </div>
                        <div class='footer'>
                            © {DateTime.Now.Year} <b>TruyenVerse Team</b>. Mọi quyền được bảo lưu.<br>
                            Hệ thống bảo mật tự động của nền tảng đọc truyện hàng đầu.
                        </div>
                    </div>
                </div>
            </body>
            </html>");
            return html.ToString();
        }
    }
}