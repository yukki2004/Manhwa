# 📚 TruyenVerse - Modern Manhwa Platform
TruyenVerse là một nền tảng đọc truyện tranh (manhwa) hiện đại được xây dựng trên hệ sinh thái .NET 8. Dự án được thiết kế theo mô hình Monolithic (Đơn khối) nhưng tuân thủ nghiêm ngặt các nguyên tắc của Clean Architecture, giúp đảm bảo tính tách biệt giữa các tầng nghiệp vụ, dễ dàng mở rộng và bảo trì trong tương lai.
Hệ thống vận hành mạnh mẽ dựa trên kiến trúc Event-Driven (Hướng sự kiện), tận dụng các tác vụ bất đồng bộ qua RabbitMQ để xử lý các logic phức tạp ngầm, từ đó tối ưu hóa tốc độ phản hồi và trải nghiệm người dùng trên hạ tầng Google Cloud Platform (GCP).

# 🌐 Trải nghiệm trực tiếp tại: [truyenverse.top](https://truyenverse.top/)

# ✨ Điểm nhấn kiến trúc

Clean Architecture Implementation: Phân tách rõ rệt giữa Domain, Application, Infrastructure và Web API, giúp mã nguồn sạch và có thể kiểm thử dễ dàng.

Modern Monolith: Một khối duy nhất để đơn giản hóa việc triển khai nhưng vẫn đảm bảo tính module hóa cao thông qua các Consumers xử lý sự kiện biệt lập.

High Performance: Kết hợp Redis Distributed Cache và Cloudflare R2 để tăng tốc độ tải trang và tối ưu hóa tài nguyên hình ảnh.

Real-time Capabilities: Đẩy thông báo và cập nhật dữ liệu tức thì cho người dùng thông qua SignalR.

# 🏗 System Architecture (Hệ thống Kiến trúc)

<img width="8192" height="3722" alt="Google Cloud Platform-2026-02-21-064630" src="https://github.com/user-attachments/assets/0af81788-13da-4541-add4-fe8351e4be75" />

# 📂 Project Structure (Cấu trúc thư mục)


