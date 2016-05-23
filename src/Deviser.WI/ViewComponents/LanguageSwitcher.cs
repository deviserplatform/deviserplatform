using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using Deviser.WI.ViewModels.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.WI.ViewComponents
{
    [ViewComponent(Name = "LanguageSwitcher")]
    public class LanguageSwitcher : DeviserViewComponent
    {
        private ILanguageProvider languageProvider;
        public LanguageSwitcher(ILifetimeScope container)
        {
            languageProvider = container.Resolve<ILanguageProvider>();
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = languageProvider.GetLanguages();
            languages = languages.Where(l => l.IsActive).ToList();
            List<LanguageViewModel> viewModel = new List<LanguageViewModel>();
            foreach (var lang in languages)
            {
                viewModel.Add(new LanguageViewModel
                {
                    Culture = new CultureInfo(lang.CultureCode),
                    EnglishName = lang.EnglishName,
                    NativeName = lang.NativeName,
                    Url = GetLocalizedUrl(lang.CultureCode),
                    IsActive = lang.CultureCode.ToLower() == AppContext.CurrentCulture.ToString().ToLower()
                });
            }
            return View(viewModel);
        }

        private string GetLocalizedUrl(string cultureCode)
        {
            if (AppContext != null && AppContext.CurrentPage != null && AppContext.CurrentPage.PageTranslation != null)
            {
                PageTranslation translation = null;
                if (AppContext.CurrentPage.PageTranslation.Any(t => t.Locale.ToLower() == cultureCode.ToLower()))
                {
                    translation = AppContext.CurrentPage.PageTranslation.First(t => t.Locale.ToLower() == cultureCode.ToLower());
                    return "/" + translation.URL;
                }
            }
            return "";
        }
    }
}
