using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Modules.UserManagement
{
    public class PasswordReset
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
