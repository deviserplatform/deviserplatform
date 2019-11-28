using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Admin.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Deviser.Admin.Config;
using Deviser.Core.Common.Module;
using Deviser.Admin.Properties;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Deviser.Admin.Services
{
    public class CoreAdminService : ICoreAdminService
    {
        private readonly IAdminSiteProvider _adminSiteProvider;
        private readonly IAdminSite _adminSite;
        private readonly ILogger<CoreAdminService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IModuleRegistry _moduleRegistry;
        private readonly IAdminRepository _adminRepository;
        JsonSerializer _serializer = new JsonSerializer();

        public CoreAdminService(string moduleName, IServiceProvider serviceProvider)
        {
            _adminSiteProvider = serviceProvider.GetService<IAdminSiteProvider>();
            _logger = serviceProvider.GetService<ILogger<CoreAdminService>>();
            _moduleRegistry = serviceProvider.GetService<IModuleRegistry>();
            _serviceProvider = serviceProvider;

            _serializer.Converters.Add(new Core.Common.Json.GuidConverter());

            var adminConfiguratorType = _moduleRegistry.GetModuleMetaInfoByModuleName(moduleName)?.AdminConfiguratorTypeInfo;
            if (adminConfiguratorType == null)
            {
                throw new ArgumentNullException(string.Format(Resources.AdminConfiguratorNotFound, moduleName));
            }
            _adminSite = _adminSiteProvider.GetAdminConfig(adminConfiguratorType.AsType());

            if(_adminSite.AdminType == AdminType.Entity)
            {
                _adminRepository = new AdminRepository(_adminSite, _serviceProvider);
            }
        }



        public async Task<object> CreateItemFor(Type modelType, object item)
        {
            return await CallGenericMethod(nameof(CreateItem), new Type[] { modelType }, new object[] { item });
        }

        public async Task<object> DeleteItemFor(Type modelType, string itemId)
        {
            return await CallGenericMethod(nameof(DeleteItem), new Type[] { modelType }, new object[] { itemId });
        }

        public IAdminConfig GetAdminConfig(string strModelType)
        {
            var modelType = GetModelType(strModelType);
            return GetAdminConfig(modelType);
        }

        public async Task<object> GetAllFor(Type modelType, int pageNo, int pageSize, string orderByProperties)
        {
            return await CallGenericMethod(nameof(GetAll), new Type[] { modelType }, new object[] { pageNo, pageSize, orderByProperties });
        }

        public async Task<object> GetItemFor(Type modelType, string itemId)
        {
            return await CallGenericMethod(nameof(GetItem), new Type[] { modelType }, new object[] { itemId });
        }

        public Type GetModelType(string strModelType)
        {
            return _adminSite.AdminConfigs.Keys.FirstOrDefault(t => t.Name == strModelType);
        }

        public async Task<object> UpdateItemFor(Type modelType, object item)
        {
            return await CallGenericMethod(nameof(UpdateItem), new Type[] { modelType }, new object[] { item });
        }


        private async Task<TModel> CreateItem<TModel>(object item) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            TModel modelToAdd = ((JObject)item).ToObject<TModel>(_serializer);
            if (_adminSite.AdminType == AdminType.Custom)
            {                
                if (_serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService)
                {
                    return await adminService.CreateItem(modelToAdd);
                }
                throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
            else //if (_adminSite.AdminType == AdminType.Entity)
            {
                return await _adminRepository.CreateItemFor<TModel>(modelToAdd);
            }
        }

        private async Task<TModel> DeleteItem<TModel>(string itemId) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            if (_adminSite.AdminType == AdminType.Custom)
            {
                if (_serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService)
                {
                    return await adminService.DeleteItem(itemId);
                }
                throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
            else //if (_adminSite.AdminType == AdminType.Entity)
            {
                return await _adminRepository.DeleteItemFor<TModel>(itemId);
            }
        }

        private async Task<PagedResult<TModel>> GetAll<TModel>(int pageNo, int pageSize, string orderByProperties) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            if (_adminSite.AdminType == AdminType.Custom)
            {
                if (_serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService)
                {
                    return await adminService.GetAll(pageNo, pageSize, orderByProperties);
                }
                throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
            else //if (_adminSite.AdminType == AdminType.Entity)
            {
                return await _adminRepository.GetAllFor<TModel>(pageNo, pageSize, orderByProperties);
            }
        }

        private async Task<TModel> GetItem<TModel>(string itemId) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            if (_adminSite.AdminType == AdminType.Custom)
            {
                if (_serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService)
                {
                    return await adminService.GetItem(itemId);
                }
                throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
            else //if (_adminSite.AdminType == AdminType.Entity)
            {
                return await _adminRepository.GetItemFor<TModel>(itemId);
            }
        }

        private async Task<TModel> UpdateItem<TModel>(object item) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            TModel modelToUpdate = ((JObject)item).ToObject<TModel>(_serializer);
            if (_adminSite.AdminType == AdminType.Custom)
            {
                if (_serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService)
                {
                    return await adminService.UpdateItem(modelToUpdate);
                }
                throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
            else //if (_adminSite.AdminType == AdminType.Entity)
            {
                return await _adminRepository.UpdateItemFor<TModel>(modelToUpdate);
            }
        }

        private IAdminConfig GetAdminConfig(Type modelType)
        {
            if (_adminSite.AdminConfigs.TryGetValue(modelType, out IAdminConfig adminConfig))
            {
                return adminConfig;
            }
            throw new InvalidOperationException(string.Format(Resources.ModelNotFoundInvalidOperation, modelType));
        }

        private async Task<object> CallGenericMethod(string methodName, Type[] genericTypes, object[] parmeters)
        {
            var getItemMethodInfo = typeof(CoreAdminService).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericTypes);
            var result = (object)await  (dynamic) getItemMethod.Invoke(this, parmeters);
            return result;
        }
    }
}
