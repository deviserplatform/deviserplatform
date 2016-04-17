using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.ViewModels.Components
{
    public class LanguageViewModel
    {
        public CultureInfo Culture { get; set; }
        public string Url { get; set; }
        public string EnglishName { get; set; }
        public string NativeName { get; set; }
        public bool IsActive { get; set; }
    }
}
