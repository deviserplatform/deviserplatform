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
        private readonly ILayoutManager _layoutManager;
        private readonly IPageRepository _pageRepository;
        private readonly ISiteSettingRepository _siteSettingRepository;
        private readonly IThemeManager _themeManager;

        public SiteSettingAdminService(ILogger<SiteSettingAdminService> logger,
            ISettingManager settingManager)
        {
            _logger = logger;
            _settingManager = settingManager;
        }

        public async Task<SiteSettingInfo> GetModel()
        {
            var siteSettings = _settingManager.GetSiteSetting();
            return await Task.FromResult(siteSettings);
        }

        public async Task<SiteSettingInfo> SaveModel(SiteSettingInfo item)
        {
            var result = _settingManager.UpdateSettingInfo(item);
            return await Task.FromResult(result);
        }

        public List<Page> GetPages()
        {
            var result = _pageRepository.GetPages();
            return result;
        }

        public List<Theme> GetThemes()
        {
            var themes = _themeManager.GetHostThemes().Select(kvp => new Theme() { Key = kvp.Key, Value = kvp.Value })
                .ToList();
            return themes;
        }
    }
}
