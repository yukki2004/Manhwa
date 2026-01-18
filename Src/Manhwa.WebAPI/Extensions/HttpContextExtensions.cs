namespace Manhwa.WebAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetRemoteIpAddress(this HttpContext context)
        {
            // Kiểm tra IP từ Connection
            var ip = context.Connection.RemoteIpAddress;

            if (ip == null) return "Unknown";

            // Xử lý trường hợp IPv6 của Localhost (::1) chuyển về IPv4 (127.0.0.1)
            if (ip.IsIPv4MappedToIPv6)
            {
                return ip.MapToIPv4().ToString();
            }

            // Nếu là địa chỉ loopback IPv6 (::1)
            if (ip.ToString() == "::1")
            {
                return "127.0.0.1";
            }

            return ip.ToString();
        }
    }
}
