using Deviser.Core.Library.Sites;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Modules.Security.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        ISettingManager settingManager;

        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor, ISettingManager settingManager)
        {
            Options = optionsAccessor.Value;
            this.settingManager = settingManager;
        }

        public AuthMessageSenderOptions Options { get; }  // set only via Secret Manager


        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSetting = settingManager.GetSMTPSetting();
            // Plug in your email service here to send an email.
            var myMessage = new MimeMessage();
            myMessage.To.Add(new MailboxAddress("", email));
            myMessage.From.Add(new MailboxAddress("Deviser", "noreply@skydevise.com"));
            myMessage.Subject = subject;
            myMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtpSetting.Server, smtpSetting.Port, smtpSetting.EnableSSL);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(smtpSetting.Username, smtpSetting.Password);
                client.Send(myMessage);
                client.Disconnect(true);
            }

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
