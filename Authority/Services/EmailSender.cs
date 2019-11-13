﻿using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Authority.Services
{
    public class EmailSender
    {
        public string HtmlMessage { get; set; }
        public IConfiguration Configuration { get; protected set; }

        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Configuration["EmailSender:FromName"], Configuration["EmailSender:From"]));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            if (HtmlMessage != "") bodyBuilder.HtmlBody = HtmlMessage;
            bodyBuilder.TextBody = text;

            message.Body = bodyBuilder.ToMessageBody();

            Int32.TryParse(Configuration["EmailSender:Port"], out int port);
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(Configuration["EmailSender:Server"], port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                client.Authenticate(Configuration["EmailSender:Username"], Configuration["EmailSender:Password"]);
                client.Send(message);
                client.Disconnect(true);
                return Task.FromResult(0);
            }
        }
    }
}
