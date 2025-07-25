﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Core.Models.Email;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _from;
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _from = configuration["EmailConfiguration:From"]!;
            _smtpServer = configuration["EmailConfiguration:SmtpServer"]!;
            _userName = configuration["EmailConfiguration:UserName"]!;
            _port = int.Parse(configuration["EmailConfiguration:Port"]!);
            _password = configuration["EmailConfiguration:Password"]!;
        }

        public async Task SendAsync(EmailRequest emailRequest)
        {
            var emailMessage = CreateEmailMessage(emailRequest);
            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailRequest emailRequest)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("no-reply", _from));
            emailMessage.To.AddRange(emailRequest.ToEmails.Select(email => MailboxAddress.Parse(email)));
            emailMessage.Subject = emailRequest.SubjectLine;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailRequest.MessageBody
            };

            if (emailRequest.FileAttachment != null)
            {
                foreach (var attachment in emailRequest.FileAttachment)
                {
                    bodyBuilder.Attachments.Add(attachment.FileName, attachment.FileContent, ContentType.Parse(attachment.MediaType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        private async Task Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Timeout = 10000; // optional
                await client.ConnectAsync(_smtpServer, _port, SecureSocketOptions.StartTls); // 587 + TLS
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_userName, _password);
                await client.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
