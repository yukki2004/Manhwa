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

```
Src/
├── Manhwa.Domain/              # Lớp lõi (Core Layer)
│   ├── Entities/               # Định nghĩa thực thể (User, Story, Chapter...)
│   ├── Enums/                  # Các hằng số định danh hệ thống
│   ├── Exceptions/             # Các ngoại lệ tùy chỉnh cho nghiệp vụ
│   └── Repositories/           # Các Interface định nghĩa cách truy xuất dữ liệu
│
├── Manhwa.Application/         # Lớp xử lý nghiệp vụ (Use Cases)
│   ├── Common/                 # Các thành phần dùng chung (DTOs, Mappers...)
│   ├── Features/               # Triển khai logic theo tính năng (Story, User, Report...)
│   └── DependencyInjection.cs  # Đăng ký các dịch vụ thuộc lớp Application
│
├── Manhwa.Infrastructure/      # Lớp hạ tầng (External Concerns)
│   ├── Persistence/            # Triển khai truy cập Database (Entity Framework Core)
│   ├── Messaging/              # Xử lý Event Bus và RabbitMQ Consumer
│   ├── Caching/                # Triển khai bộ nhớ đệm với Redis
│   ├── BackgroundTasks/        # Các Worker xử lý các tác vụ chạy ngầm
│   ├── FileStorage/            # Quản lý lưu trữ tài nguyên qua Cloudflare R2
│   ├── Realtime/               # Xử lý giao tiếp thời gian thực qua SignalR
│   ├── Identity/               # Quản lý xác thực và phân quyền người dùng
│   └── Notifications/          # Chiến lược gửi thông báo (Email, Push)
│
└── Manhwa.WebAPI/              # Lớp trình diễn (Presentation)
    ├── Controllers/            # Định nghĩa các Endpoint RESTful API
    ├── Middleware/             # Xử lý yêu cầu HTTP (Logging, Error Handling)
    ├── Program.cs              # Điểm khởi đầu của ứng dụng và cấu hình Pipeline
    └── Dockerfile              # Cấu hình đóng gói Container cho quá trình triển khai
```

# 🚀 Hướng dẫn cài đặt (Installation Guide)

## Bước 1: Clone mã nguồn dự án

```
git clone https://github.com/yukki2004/Manhwa.git
cd Manhwa
```
## Bước 2: Cấu hình biến môi trường

Tạo tệp .env tại thư mục gốc của dự án và cấu hình các thông số cần thiết dựa trên các dịch vụ hạ tầng:

```
# Database Configuration (PostgreSQL)
DB_CONNECTION_STRING=Host=db;Database=ManhwaDb;Username=postgres;Password=your_password

# Caching (Redis)
REDIS_URL=redis:6379

# Messaging (RabbitMQ)
RABBITMQ_HOST=rabbitmq
RABBITMQ_USER=guest
RABBITMQ_PASS=guest

# Cloud Storage (Cloudflare R2)
R2_ACCESS_KEY=your_access_key
R2_SECRET_KEY=your_secret_key
R2_BUCKET_NAME=your_bucket_name

# JWT Identity
JWT_SECRET=your_super_secret_key_here

```
## Bước 3: Khởi chạy hệ thống với Docker Compose

```
docker-compose up -d

```

# 🏗️ Kiến trúc & Công nghệ sử dụng

| Tầng kiến trúc | Công nghệ / Công cụ | Mô tả và Vai trò |
|---|---|---|
| **Presentation** | .NET 8/9 Web API | Xây dựng hệ thống RESTful API mạnh mẽ và bảo mật. |
|  | ReactJS & Tailwind CSS | Phát triển giao diện người dùng hiện đại, tối ưu hóa trải nghiệm đọc truyện. |
| **Application** | MediatR (CQRS) | Tách biệt xử lý logic nghiệp vụ theo từng tính năng (Features). |
|  | FluentValidation | Đảm bảo tính toàn vẹn và hợp lệ của dữ liệu đầu vào. |
| **Domain (Core)** | C# Entities & Enums | Định nghĩa các thực thể cốt lõi và các quy tắc nghiệp vụ của hệ thống. |
| **Infrastructure** | Entity Framework Core | Truy xuất và quản lý dữ liệu thông qua cơ chế ORM hiện đại. |
|  | RabbitMQ | Message Broker điều phối xử lý các tác vụ nền (Background Tasks) bất đồng bộ. |
|  | Redis Cluster | Quản lý bộ nhớ đệm (Cache) phân tán để tối ưu hóa hiệu năng truy xuất. |
|  | SignalR | Cung cấp khả năng giao tiếp thời gian thực (Real-time) cho các thông báo. |
| **Data & Assets** | PostgreSQL | Hệ quản trị cơ sở dữ liệu quan hệ chính lưu trữ dữ liệu người dùng và truyện. |
|  | Cloudflare R2 | Lưu trữ tệp tin hình ảnh truyện theo cơ chế Object Storage (S3 Compatible). |
| **DevOps & Cloud** | GCP (Compute Engine) | Hạ tầng máy chủ ảo vận hành toàn bộ hệ thống trên nền tảng điện toán đám mây. |
|  | Docker & Compose | Đóng gói ứng dụng thành các Container để đồng nhất môi trường triển khai. |
|  | GitHub Actions | Tự động hóa quy trình CI/CD từ xây dựng đến triển khai trực tiếp lên server. |






