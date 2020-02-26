using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Admin.Properties;
using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Module;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

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
        private readonly JsonSerializer _serializer;

        public CoreAdminService(string moduleName, IServiceProvider serviceProvider)
        {
            _adminSiteProvider = serviceProvider.GetService<IAdminSiteProvider>();
            _logger = serviceProvider.GetService<ILogger<CoreAdminService>>();
            _moduleRegistry = serviceProvider.GetService<IModuleRegistry>();
            _serviceProvider = serviceProvider;

            _serializer = new JsonSerializer();
            _serializer.NullValueHandling = NullValueHandling.Ignore;
            _serializer.Converters.Add(new Core.Common.Json.GuidConverter());


            var adminConfiguratorType = _moduleRegistry.GetModuleMetaInfoByModuleName(moduleName)?.AdminConfiguratorTypeInfo;
            if (adminConfiguratorType == null)
            {
                throw new ArgumentNullException(string.Format(Resources.AdminConfiguratorNotFound, moduleName));
            }
            _adminSite = _adminSiteProvider.GetAdminConfig(adminConfiguratorType.AsType());

            if (_adminSite.AdminType == AdminType.Entity)
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

        public async Task<object> ExecuteMainFormAction(Type modelType, string actionName, object entityObject)
        {
            var adminConfig = GetAdminConfig(modelType);
            var model = ((JObject)entityObject).ToObject(adminConfig.ModelType);
            if (adminConfig.ModelConfig.FormConfig.FormActions.TryGetValue(actionName.Pascalize(), out AdminAction adminAction) && adminConfig.ModelType == model.GetType())
            {
                return await CallGenericMethod(nameof(ExecuteAdminAction), new Type[] { model.GetType() }, new object[] { model, adminAction });
            }
            return null;
        }

        public async Task<object> ExecuteCustomFormAction(Type modelType, string formName, string actionName, object entityObject)
        {
            var adminConfig = GetAdminConfig(modelType);
            var model = ((JObject)entityObject).ToObject(adminConfig.ModelType);
            if (adminConfig.ModelConfig.CustomForms.TryGetValue(formName.Pascalize(), out CustomForm customForm) &&
                customForm.FormConfig.FormActions.TryGetValue(actionName.Pascalize(), out AdminAction adminAction) &&
                adminConfig.ModelType == model.GetType())
            {
                return await CallGenericMethod(nameof(ExecuteAdminAction), new Type[] { model.GetType() }, new object[] { model, adminAction });
            }
            return null;
        }

        public async Task<object> CustomFormSubmit(string strModelType, string formName, object entityObject)
        {
            var adminConfig = GetAdminConfig(strModelType);
            var modelType = GetCustomFormModelType(strModelType, formName);
            var model = ((JObject)entityObject).ToObject(modelType);
            if (adminConfig.ModelConfig.CustomForms.TryGetValue(formName.Pascalize(), out CustomForm customForm) &&
                customForm.ModelType == model.GetType())
            {
                return await CallGenericMethod(nameof(ExecuteCustomFormSubmitAction), new Type[] { model.GetType() }, new object[] { model, customForm });
            }
            return null;
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

        public Type GetCustomFormModelType(string strModelType, string formName)
        {
            var adminConfig = GetAdminConfig(strModelType);
            if (adminConfig.ModelConfig.CustomForms.TryGetValue(formName.Pascalize(), out CustomForm customForm))
            {
                return customForm.ModelType;
            }

            return null;
        }

        public async Task<object> UpdateItemFor(Type modelType, object item)
        {
            return await CallGenericMethod(nameof(UpdateItem), new Type[] { modelType }, new object[] { item });
        }
        
        public async Task<ValidationResult> ExecuteMainFormCustomValidation(Type modelType, string fieldName, object fieldObject)
        {
            var adminConfig = GetAdminConfig(modelType);
            var field = adminConfig.ModelConfig.FormConfig.AllFormFields.FirstOrDefault(f =>
                string.Equals(f.FieldName, fieldName, StringComparison.InvariantCultureIgnoreCase));
            
            return await CustomValidation(fieldObject, field);
        }

        public async Task<ValidationResult> ExecuteChildFormCustomValidation(Type modelType, string formName, string fieldName, object fieldObject)
        {
            var adminConfig = GetAdminConfig(modelType);

            var childConfig = adminConfig.ChildConfigs.FirstOrDefault(c =>
                //Child form name is same as field parent form
                c.Field.FieldName.Equals(formName, StringComparison.InvariantCultureIgnoreCase));

            if (childConfig == null)
            {
                return null;
            }


            var field = childConfig.ModelConfig.FormConfig.AllFormFields.FirstOrDefault(f =>
                string.Equals(f.FieldName, fieldName, StringComparison.InvariantCultureIgnoreCase));

            return await CustomValidation(fieldObject, field);
        }

        public async Task<ValidationResult> ExecuteCustomFormCustomValidation(Type modelType, string formName, string fieldName, object fieldObject)
        {
            var adminConfig = GetAdminConfig(modelType);


            if (!adminConfig.ModelConfig.CustomForms.ContainsKey(formName))
            {
                return null;
            }
            formName = formName.Pascalize();
            var customForm = adminConfig.ModelConfig.CustomForms[formName];

            var field = customForm.FormConfig.AllFormFields.FirstOrDefault(f =>
                string.Equals(f.FieldName, fieldName, StringComparison.InvariantCultureIgnoreCase));
            
            return await CustomValidation(fieldObject, field);
        }

        private async Task<ValidationResult> CustomValidation(object fieldObject, Field field)
        {
            if (field == null || field.FieldOption.ValidationType != ValidationType.Custom ||
                field.FieldOption.ValidationExpression == null)
            {
                return null;
            }

            var fieldValue = (fieldObject.GetType() == field.FieldClrType)? Convert.ChangeType(fieldObject, field.FieldClrType) : ((JObject) fieldObject).ToObject(field.FieldClrType);
            var validationDelegate = field.FieldOption.ValidationExpression.Compile();
            var result = await (dynamic) validationDelegate.DynamicInvoke(_serviceProvider, fieldValue) as ValidationResult;
            return result;
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
            if (getItemMethodInfo != null)
            {
                var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericTypes);
                var result = (object)await (dynamic)getItemMethod.Invoke(this, parmeters);
                return result;
            }

            return null;
        }

        private async Task<IFormResult> ExecuteAdminAction<TModel>(TModel entityObject, AdminAction adminAction) where TModel : class
        {
            var formActionDelegate = adminAction.FormActionExpression.Compile();
            var result = await (dynamic)formActionDelegate.DynamicInvoke(_serviceProvider, entityObject) as IFormResult;
            return result;
        }

        private async Task<IFormResult> ExecuteCustomFormSubmitAction<TModel>(TModel entityObject, CustomForm customForm) where TModel : class
        {
            var formActionDelegate = customForm.SubmitActionExpression.Compile();
            var result = await (dynamic)formActionDelegate.DynamicInvoke(_serviceProvider, entityObject) as IFormResult;
            return result;
        }
    }
}
