using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library;
using Deviser.Core.Library.Extensions;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Deviser.Web.ViewModels.Components;

namespace Deviser.Web.ViewComponents
{
    [ViewComponent(Name = "LanguageSwitcher")]
    public class LanguageSwitcher : DeviserViewComponent
    {
        private ILanguageRepository _languageRepository;
        private readonly IScopeService _scopeService;
        public LanguageSwitcher(IScopeService scopeService, 
            ILanguageRepository languageRepository)
            :base(scopeService)
        {
            _languageRepository = languageRepository;
            _scopeService = scopeService;
        }

        public override async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = _languageRepository.GetLanguages();
            languages = languages.Where(l => l.IsActive).ToList();
            var viewModel = new List<LanguageViewModel>();
            foreach (var lang in languages)
            {
                viewModel.Add(new LanguageViewModel
                {
                    Culture = new CultureInfo(lang.CultureCode),
                    EnglishName = lang.EnglishName,
                    NativeName = lang.NativeName,
                    Url = GetLocalizedUrl(lang.CultureCode),
                    IsActive = lang.CultureCode.ToLower() == _scopeService.PageContext.CurrentCulture.ToString().ToLower()
                });
            }
            return View(viewModel);
        }

        private string GetLocalizedUrl(string cultureCode)
        {
            if (_scopeService.PageContext?.CurrentPage?.PageTranslation == null) return "";

            PageTranslation translation = null;
            if (_scopeService.PageContext.CurrentPage.PageTranslation.All(t => t.Locale.ToLower() != cultureCode.ToLower())) return "";

            translation = _scopeService.PageContext.CurrentPage.PageTranslation.Get(cultureCode.ToLower());
            return "/" + translation.URL;
        }
    }
}
