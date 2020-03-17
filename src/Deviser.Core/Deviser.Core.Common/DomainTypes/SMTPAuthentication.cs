using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class SMTPAuthentication
    {
        private static IList<SMTPAuthentication> _smtpAuthentication;

        public int Id { get; set; }
        public string Name { get; set; }

        public static IList<SMTPAuthentication> GetSmtpAuthentications()
        {
            return _smtpAuthentication ??= new List<SMTPAuthentication>()
            {
                new SMTPAuthentication()
                {
                    Id = 1,
                    Name = "Anonymous"
                },
                new SMTPAuthentication()
                {
                    Id = 2,
                    Name = "Basic"
                },
                new SMTPAuthentication()
                {
                    Id = 3,
                    Name = "NTML"
                }
            };
        }
    }
}
