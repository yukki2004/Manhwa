using Manhwa.Application.Common.Interfaces;
using System.Text;

namespace Manhwa.Infrastructure.Messaging
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GenerateHtmlBody(string templateName, Dictionary<string, string> data)
        {
            string specificContent = GetSpecificContent(templateName, data);

            // Nhúng vào Base Layout với tiêu đề tương ứng
            return WrapWithBaseLayout(data.GetValueOrDefault("Subject", "Thông báo từ TruyenVerse"), specificContent);
        }

        private string GetSpecificContent(string name, Dictionary<string, string> data) => name switch
        {
            "WELCOME_NEW_USER" => $@"
                <div class='section'>
                    <p class='greeting'>Chào <b>{data.GetValueOrDefault("Username", "Đạo hữu")}</b> thân mến! 🚀</p>
                    <p class='main-text'>Chào mừng bạn đã chính thức gia nhập <b>TruyenVerse</b>. Chúng tôi rất vinh dự khi được đồng hành cùng bạn trên hành trình khám phá những thế giới kỳ ảo nhất qua từng trang truyện.</p>
                    
                    <div class='feature-card'>
                        <h3 class='card-title'>Bạn có thể làm gì tại TruyenVerse?</h3>
                        <div class='feature-item'>
                            <span class='icon'>📚</span>
                            <div><b>Thư viện khổng lồ:</b> Truy cập hàng ngàn bộ Manhwa, Manga và Novel mới nhất được cập nhật liên tục.</div>
                        </div>
                        <div class='feature-item'>
                            <span class='icon'>💬</span>
                            <div><b>Thảo luận sôi nổi:</b> Giao lưu, kết bạn và chia sẻ cảm xúc cùng cộng đồng đạo hữu văn minh.</div>
                        </div>
                        <div class='feature-item'>
                            <span class='icon'>🔥</span>
                            <div><b>Hệ thống Level:</b> Đọc truyện tích lũy EXP, thăng cấp và khẳng định vị thế trong giới.</div>
                        </div>
                        <div class='feature-item'>
                            <span class='icon'>⭐</span>
                            <div><b>Rating & Review:</b> Đánh giá bộ truyện 'ruột' để ủng hộ tác giả và nhận đề xuất truyện hay.</div>
                        </div>
                    </div>

                    <div style='text-align: center; margin: 35px 0;'>
                        <a href='https://truyenverse.com' class='btn-cta'>BẮT ĐẦU KHÁM PHÁ NGAY</a>
                    </div>

                    <p class='sub-text'>Nếu bạn có bất kỳ thắc mắc nào, đừng ngần ngại tham gia cộng đồng <b>Discord</b> hoặc phản hồi trực tiếp qua email này nhé!</p>
                </div>",

            "OTP_VERIFY" => $@"
                <div class='section'>
                    <p class='main-text'>Bạn đang thực hiện thao tác bảo mật quan trọng. Vui lòng sử dụng mã xác thực dưới đây để hoàn tất. <b>Lưu ý:</b> mã sẽ hết hạn sau 5 phút.</p>
                    <div class='otp-display'>{data.GetValueOrDefault("OtpCode", "000000")}</div>
                    <p class='sub-text' style='text-align: center;'>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email để đảm bảo an toàn cho tài khoản.</p>
                </div>",

            "PASSWORD_CHANGED" => $@"
                <div class='section'>
                    <p class='main-text'>Chào <b>{data.GetValueOrDefault("Username", "Bạn")}</b>, mật khẩu tài khoản TruyenVerse của bạn vừa được thay đổi thành công.</p>
                    <div class='info-table'>
                        <div class='info-row'><span>Thời gian:</span> <b>{DateTime.Now:HH:mm:ss dd/MM/yyyy} (UTC)</b></div>
                        <div class='info-row'><span>Địa chỉ IP:</span> <b>{data.GetValueOrDefault("IpAddress", "Unknown")}</b></div>
                        <div class='info-row'><span>Thiết bị:</span> <b>{data.GetValueOrDefault("UserAgent", "Unknown")}</b></div>
                    </div>
                    <div class='alert-banner'>
                        <b>Bạn không thực hiện thay đổi này?</b><br>
                        Tài khoản của bạn có thể đang gặp nguy hiểm. Hãy sử dụng chức năng Quên mật khẩu ngay lập tức.
                    </div>
                </div>",

            "ACCOUNT_LOCKED" => $@"
                <div class='section'>
                    <p class='main-text'>Chào {data.GetValueOrDefault("Username", "Bạn")}, tài khoản của bạn đã bị <b>TẠM KHÓA</b> để đảm bảo an toàn hệ thống.</p>
                    <div class='info-table' style='border-left: 4px solid #ef4444;'>
                        <div class='info-row'><span>Lý do:</span> <b>{data.GetValueOrDefault("Reason", "Vi phạm quy định cộng đồng")}</b></div>
                    </div>
                    <p class='main-text'>Vui lòng liên hệ bộ phận hỗ trợ hoặc tham gia Discord để được giải quyết nhanh nhất.</p>
                </div>",

            _ => $"<div class='section'><p class='main-text'>{data.GetValueOrDefault("Content", "Có thông báo mới dành cho bạn từ TruyenVerse.")}</p></div>"
        };

        private string WrapWithBaseLayout(string title, string content)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='vi'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <style>
                    /* Typography & Colors - Optimized for Readability */
                    body {{ font-family: 'Inter', -apple-system, 'Segoe UI', Roboto, sans-serif; background-color: #020617; margin: 0; padding: 0; color: #f1f5f9; -webkit-font-smoothing: antialiased; }}
                    .container {{ max-width: 600px; margin: 40px auto; background-color: #0f172a; border-radius: 24px; border: 1px solid #1e293b; overflow: hidden; box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5); }}
                    
                    /* Header Area */
                    .header {{ background: linear-gradient(135deg, #4f46e5 0%, #9333ea 100%); padding: 40px; text-align: center; }}
                    .logo {{ font-size: 28px; font-weight: 900; color: #ffffff; letter-spacing: 2px; text-transform: uppercase; text-shadow: 0 2px 4px rgba(0,0,0,0.3); }}
                    
                    /* Content Area */
                    .body {{ padding: 50px 40px; }}
                    .email-title {{ font-size: 26px; font-weight: 800; color: #ffffff; margin-bottom: 30px; line-height: 1.2; text-align: center; }}
                    .greeting {{ font-size: 19px; color: #f8fafc; margin-bottom: 16px; }}
                    .main-text {{ font-size: 16px; line-height: 1.8; color: #cbd5e1; margin-bottom: 24px; }}
                    .sub-text {{ font-size: 14px; color: #94a3b8; line-height: 1.6; margin-top: 30px; }}
                    
                    /* Components */
                    .feature-card {{ background-color: #1e293b; border-radius: 16px; padding: 28px; border: 1px solid #334155; margin: 35px 0; }}
                    .card-title {{ font-size: 17px; color: #818cf8; margin-top: 0; margin-bottom: 20px; font-weight: 700; text-transform: uppercase; }}
                    .feature-item {{ display: flex; align-items: flex-start; margin-bottom: 16px; font-size: 15px; color: #e2e8f0; line-height: 1.6; }}
                    .icon {{ margin-right: 14px; font-size: 20px; min-width: 24px; text-align: center; }}
                    
                    .otp-display {{ font-size: 44px; font-weight: 800; color: #ffffff; letter-spacing: 12px; text-align: center; margin: 35px 0; padding: 30px; background: #020617; border-radius: 16px; border: 2px solid #4f46e5; text-shadow: 0 0 15px rgba(79, 70, 229, 0.4); }}
                    
                    .info-table {{ background: #020617; padding: 20px; border-radius: 12px; margin-bottom: 25px; border: 1px solid #1e293b; }}
                    .info-row {{ display: flex; justify-content: space-between; font-size: 14px; padding: 8px 0; border-bottom: 1px solid #1e293b; }}
                    .info-row:last-child {{ border-bottom: none; }}
                    .info-row span {{ color: #64748b; }}
                    .info-row b {{ color: #f1f5f9; }}
                    
                    .alert-banner {{ background-color: rgba(239, 68, 68, 0.1); border-left: 4px solid #ef4444; padding: 18px; border-radius: 8px; color: #fca5a5; font-size: 14px; line-height: 1.6; }}
                    
                    .btn-cta {{ display: inline-block; padding: 16px 36px; background-color: #ffffff; color: #020617 !important; text-decoration: none; border-radius: 14px; font-weight: 800; font-size: 16px; box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.3); }}
                    
                    /* Footer Area */
                    .footer {{ padding: 35px; text-align: center; font-size: 12px; color: #475569; background-color: #020617; border-top: 1px solid #1e293b; }}
                    .footer b {{ color: #64748b; }}
                </style>
            </head>
            <body>
                <div class='wrapper'>
                    <div class='container'>
                        <div class='header'><div class='logo'>TruyenVerse</div></div>
                        <div class='body'>
                            <h1 class='email-title'>{title}</h1>
                            {content}
                        </div>
                        <div class='footer'>
                            © {DateTime.Now.Year} <b>TruyenVerse Team</b>. Mọi quyền được bảo lưu.<br>
                            Hệ thống bảo mật tự động, vui lòng không trả lời email này.
                        </div>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}