using System.Threading.Tasks;

namespace Deviser.Core.Library.Messaging
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string optfromaddress = null);
    }
}
