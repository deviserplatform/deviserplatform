using Deviser.Core.Library.Sites;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Messaging
{
    public class MessageSender: IEmailSender, ISmsSender
    {
        private readonly ISettingManager _settingManager;
        
        public MessageSender(ISettingManager settingManager)
        { 
            _settingManager = settingManager;
        }


        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSetting = _settingManager.GetSMTPSetting();
            var siteSetting = _settingManager.GetSiteSetting();
            // Plug in your email service here to send an email.
            var myMessage = new MimeMessage();
            myMessage.To.Add(new MailboxAddress("", email));
            myMessage.From.Add(new MailboxAddress("Deviser", siteSetting.SiteAdminEmail));
            myMessage.Subject = subject;
            myMessage.Body = new TextPart("HTML")
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
