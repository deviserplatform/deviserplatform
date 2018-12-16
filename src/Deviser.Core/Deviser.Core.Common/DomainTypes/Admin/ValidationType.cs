using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public enum ValidationType
    {
        None=0,
        Email = 1,
        NumberOnly = 2,
        LettersOnly = 3,
        Password = 4,
        UserExist = 5,
        UserExistByEmail = 6,
        RegEx = 7,
        Custom = 8
    }
}
