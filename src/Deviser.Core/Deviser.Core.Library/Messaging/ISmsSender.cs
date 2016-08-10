using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Messaging
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
