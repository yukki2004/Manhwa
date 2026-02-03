using Manhwa.Application.Common.Interfaces;
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
    public class EmailNotificationConsumer : IConsumer<SendEmailIntegrationEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailTemplateService _templateService;

        public EmailNotificationConsumer(IConfiguration configuration, IEmailTemplateService templateService)
        {
            _configuration = configuration;
            _templateService = templateService;
        }

        public async Task Consume(ConsumeContext<SendEmailIntegrationEvent> context)
        {
            var eventData = context.Message;

            string htmlBody = _templateService.GenerateHtmlBody(eventData.TemplateName, eventData.TemplateData);

            var smtp = _configuration.GetSection("EmailSettings");
            using var client = new SmtpClient(smtp["SmtpServer"], int.Parse(smtp["SmtpPort"] ?? "587"))
            {
                Credentials = new NetworkCredential(smtp["SenderEmail"], smtp["Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtp["SenderEmail"]!, smtp["SenderName"] ?? "TruyenVerse"),
                Subject = eventData.Subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(eventData.To);

            await client.SendMailAsync(mailMessage);
        }
    }
}
