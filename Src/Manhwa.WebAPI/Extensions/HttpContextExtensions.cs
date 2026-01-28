namespace Manhwa.WebAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetRemoteIpAddress(this HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress;

            if (ip == null) return "Unknown";

            if (ip.IsIPv4MappedToIPv6)
            {
                return ip.MapToIPv4().ToString();
            }

            if (ip.ToString() == "::1")
            {
                return "127.0.0.1";
            }

            return ip.ToString();
        }
        public static string GetUserIdentity(this HttpContext context)
        {
            return context.Items["UserIdentity"]?.ToString() ?? string.Empty;
        }
    }
}
