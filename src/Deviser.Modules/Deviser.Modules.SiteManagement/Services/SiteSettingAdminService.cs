using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Sites;
using Microsoft.Extensions.Logging;

namespace Deviser.Modules.SiteManagement.Services
{
    public class SiteSettingAdminService : IAdminFormService<SiteSettingInfo>
    {
        private readonly ILogger<SiteSettingAdminService> _logger;
        private readonly ISettingManager _settingManager;
        private readonly IPageRepository _pageRepository;
        private readonly IThemeManager _themeManager;

        public SiteSettingAdminService(ILogger<SiteSettingAdminService> logger,
            IPageRepository pageRepository,
            ISettingManager settingManager,
            IThemeManager themeManager)
        {
            _logger = logger;
            _pageRepository = pageRepository;
            _settingManager = settingManager;
            _themeManager = themeManager;
        }

        public async Task<SiteSettingInfo> GetModel()
        {
            var siteSettings = _settingManager.GetSiteSetting();
            return await Task.FromResult(siteSettings);
        }

        public async Task<IFormResult<SiteSettingInfo>> SaveModel(SiteSettingInfo item)
        {
            var settingInfo = _settingManager.UpdateSettingInfo(item);
            if (settingInfo != null)
            {
                var result = new FormResult<SiteSettingInfo>(settingInfo)
                {
                    IsSucceeded = true,
                    FormBehaviour = FormBehaviour.StayOnEditMode,
                    SuccessMessage = "SiteSettings has been updated successfully"
                };
                return await Task.FromResult(result);
            }
            else
            {
                var result = new FormResult<SiteSettingInfo>(settingInfo)
                {
                    IsSucceeded = false,
                    FormBehaviour = FormBehaviour.StayOnEditMode,
                    SuccessMessage = "Unable to update SiteSettings"
                };
                return await Task.FromResult(result);
            }
        }

        public IList<Page> GetPages()
        {
            var result = _pageRepository.GetPagesFlat();
            return result;
        }

        public List<Theme> GetThemes()
        {
            var themes = _themeManager.GetHostThemes().Select(kvp => new Theme() { Key = kvp.Value, Value = kvp.Key })
                .ToList();
            return themes;
        }
    }
}
