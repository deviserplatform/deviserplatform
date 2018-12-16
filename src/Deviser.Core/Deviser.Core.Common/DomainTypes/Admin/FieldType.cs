using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public enum FieldType
    {
        Unknown = 0,
        Static = 1,
        TextBox = 2,
        Number = 3,
        EmailAddress = 4,
        Phone = 5,
        TextArea = 6,
        RichText = 7,
        Date = 8,
        Time = 9,
        DateTime = 10,
        Select = 11,
        MultiSelect = 12,
        RadioButton = 13,
        MultiSelectCheckBox = 14,
        FileAttachment = 15,
        Image = 16,
        Password = 17,
        Hidden = 18,
        CheckBox = 19,
        Currency = 20,
        Url = 21,
        CreditCard = 22,
        Custom = 23,
        KeyField = 24,
    }
}
