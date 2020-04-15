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
using System.Collections;
using System.Collections.Generic;
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

        public async Task<object> ExecuteGridAction(Type modelType, string actionName, object entityObject)
        {
            var adminConfig = GetAdminConfig(modelType);
            var model = ((JObject)entityObject).ToObject(adminConfig.ModelType);
            if (adminConfig.ModelConfig.GridConfig.RowActions.TryGetValue(actionName.Pascalize(), out AdminAction adminAction) &&
                adminConfig.ModelType == model.GetType())
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

        public async Task<object> GetTree(Type modelType)
        {
            return await CallGenericMethod(nameof(GetTree), new Type[] { modelType }, null);
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

        public async Task<object> UpdateTreeFor(Type modelType, object item)
        {
            return await CallGenericMethod(nameof(UpdateTree), new Type[] { modelType }, new object[] { item });
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

        public async Task<ICollection<LookUpField>> GetLookUpForMainForm(Type modelType, string fieldName, object filterParam)
        {
            var adminConfig = GetAdminConfig(modelType);
            var relatedFiled = adminConfig.ModelConfig.FormConfig.AllFormFields.FirstOrDefault(f => f.FieldName == fieldName);
            return await GetLookUpFields(filterParam, relatedFiled);
        }

        public async Task<ICollection<LookUpField>> GetLookUpForChildForm(Type modelType, string formName, string fieldName, object filterParam)
        {
            var adminConfig = GetAdminConfig(modelType);
            var childConfig = adminConfig.ChildConfigs.FirstOrDefault(c =>
                c.ModelConfig.FormConfig.AllFormFields.Any(f => f.FieldName == fieldName));

            if (childConfig == null) return await Task.FromResult<ICollection<LookUpField>>(null);


            var relatedField = childConfig.ModelConfig.FormConfig.AllFormFields.FirstOrDefault(f => f.FieldName == fieldName);
            return await GetLookUpFields(filterParam, relatedField);
        }

        public async Task<ICollection<LookUpField>> GetLookUpForCustomForm(Type modelType, string formName,
            string fieldName, object filterParam)
        {
            var adminConfig = GetAdminConfig(modelType);

            formName = formName.Pascalize();
            var customForm = adminConfig.ModelConfig.CustomForms[formName];

            if (customForm == null) return await Task.FromResult<ICollection<LookUpField>>(null);

            var relatedField = customForm.FormConfig.AllFormFields.FirstOrDefault(f => f.FieldName == fieldName);
            return await GetLookUpFields(filterParam, relatedField);
        }

        private async Task<ICollection<LookUpField>> GetLookUpFields(object filterParam, Field relatedField)
        {
            var lookUpFields = new List<LookUpField>();

            var entityLookupExprDelegate = relatedField.FieldOption.LookupExpression.Compile();
            var entityLookupExprKeyDelegate = relatedField.FieldOption.LookupKeyExpression.Compile();
            var displayExprDelegate = relatedField.FieldOption.LookupDisplayExpression.Compile();
            var keyFieldName = ReflectionExtensions.GetMemberName(relatedField.FieldOption.LookupKeyExpression);

            var lookupFilterFieldType = relatedField.FieldOption.LookupFilterField.FieldClrType;
            var filterParamTyped = (filterParam.GetType() == lookupFilterFieldType) ? Convert.ChangeType(filterParam, lookupFilterFieldType) : ((JObject)filterParam).ToObject(lookupFilterFieldType);

            var items = entityLookupExprDelegate.DynamicInvoke(new object[] { _serviceProvider, filterParamTyped }) as IList;

            foreach (var item in items)
            {
                var keyValue = entityLookupExprKeyDelegate.DynamicInvoke(new object[] { item });
                var displayName = displayExprDelegate.DynamicInvoke(new object[] { item }) as string;
                lookUpFields.Add(new LookUpField()
                { Key = new Dictionary<string, object>() { { keyFieldName, keyValue } }, DisplayName = displayName });
            }

            return await Task.FromResult(lookUpFields);
        }

        private async Task<ValidationResult> CustomValidation(object fieldObject, Field field)
        {
            if (field == null || field.FieldOption.ValidationType != ValidationType.Custom ||
                field.FieldOption.ValidationExpression == null)
            {
                return null;
            }

            var fieldValue = (fieldObject.GetType() == field.FieldClrType) ? Convert.ChangeType(fieldObject, field.FieldClrType) : ((JObject)fieldObject).ToObject(field.FieldClrType);
            var validationDelegate = field.FieldOption.ValidationExpression.Compile();
            var result = await (dynamic)validationDelegate.DynamicInvoke(_serviceProvider, fieldValue) as ValidationResult;
            return result;
        }

        private async Task<IFormResult<TModel>> CreateItem<TModel>(object item) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            var modelToAdd = ((JObject)item).ToObject<TModel>(_serializer);

            if (_adminSite.AdminType == AdminType.Entity)
            {
                var result = await _adminRepository.CreateItemFor<TModel>(modelToAdd);
                if (result == null)
                {
                    return new FormResult<TModel>(result)
                    {
                        IsSucceeded = false,
                        ErrorMessage = $"Unable to create {adminConfig.ModelType.Name}"
                    };
                }
                return new FormResult<TModel>(result)
                {
                    IsSucceeded = true
                };
            }

            switch (adminConfig.AdminConfigType)
            {
                case AdminConfigType.GridAndForm when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService:
                    return await adminService.CreateItem(modelToAdd);
                case AdminConfigType.FormOnly when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminTreeService<TModel> adminService:
                    return await adminService.CreateItem(modelToAdd);
                default:
                    throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
        }

        private async Task<IAdminResult<TModel>> DeleteItem<TModel>(string itemId) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));

            if (_adminSite.AdminType == AdminType.Entity)
            {
                var result = await _adminRepository.DeleteItemFor<TModel>(itemId);
                if (result == null)
                {
                    return new AdminResult<TModel>(result)
                    {
                        IsSucceeded = false,
                        ErrorMessage = $"Unable to create {adminConfig.ModelType.Name}"
                    };
                }
                return new AdminResult<TModel>(result)
                {
                    IsSucceeded = true
                };
            }

            switch (adminConfig.AdminConfigType)
            {
                case AdminConfigType.GridOnly when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminGridService<TModel> adminService:
                    return await adminService.DeleteItem(itemId);
                case AdminConfigType.GridAndForm  when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService:
                    return await adminService.DeleteItem(itemId);
                case AdminConfigType.TreeAndForm when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminTreeService<TModel> adminService:
                    return await adminService.DeleteItem(itemId);
                default:
                    throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
        }

        private async Task<PagedResult<TModel>> GetAll<TModel>(int pageNo, int pageSize, string orderByProperties) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));

            if (_adminSite.AdminType == AdminType.Entity)
            {
                return await _adminRepository.GetAllFor<TModel>(pageNo, pageSize, orderByProperties);
            }

            switch (adminConfig.AdminConfigType)
            {
                case AdminConfigType.GridOnly when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminGridService<TModel> adminService:
                    return await adminService.GetAll(pageNo, pageSize, orderByProperties);
                case AdminConfigType.GridAndForm when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService:
                    return await adminService.GetAll(pageNo, pageSize, orderByProperties);
                default:
                    throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
        }

        private async Task<TModel> GetTree<TModel>() where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            var adminService = _serviceProvider.GetService(adminConfig.AdminServiceType) as IAdminTreeService<TModel>;

            if (adminService == null)
            {
                throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
            return await adminService.GetTree();
        }

        private async Task<TModel> GetItem<TModel>(string itemId) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));

            if (_adminSite.AdminType == AdminType.Entity) return await _adminRepository.GetItemFor<TModel>(itemId);

            switch (adminConfig.AdminConfigType)
            {
                case AdminConfigType.GridAndForm when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService:
                    return await adminService.GetItem(itemId);
                case AdminConfigType.FormOnly when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminFormService<TModel> adminService:
                    return await adminService.GetModel();
                case AdminConfigType.FormOnly when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminTreeService<TModel> adminService:
                    return await adminService.GetItem(itemId);
                default:
                    throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
        }

        private async Task<IFormResult<TModel>> UpdateItem<TModel>(object item) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            var modelToUpdate = ((JObject)item).ToObject<TModel>(_serializer);

            if (_adminSite.AdminType == AdminType.Entity)
            {
                var result = await _adminRepository.UpdateItemFor<TModel>(modelToUpdate);
                if (result == null)
                {
                    return new FormResult<TModel>(result)
                    {
                        IsSucceeded = false,
                        ErrorMessage = $"Unable to create {adminConfig.ModelType.Name}"
                    };
                }
                return new FormResult<TModel>(result)
                {
                    IsSucceeded = true
                };
            }

            switch (adminConfig.AdminConfigType)
            {
                case AdminConfigType.GridAndForm when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminService<TModel> adminService:
                    return await adminService.UpdateItem(modelToUpdate);
                case AdminConfigType.FormOnly when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminFormService<TModel> adminService:
                    return await adminService.SaveModel(modelToUpdate);
                case AdminConfigType.FormOnly when _serviceProvider.GetService(adminConfig.AdminServiceType) is IAdminTreeService<TModel> adminService:
                    return await adminService.UpdateItem(modelToUpdate);
                default:
                    throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
        }

        private async Task<IFormResult<TModel>> UpdateTree<TModel>(object item) where TModel : class
        {
            var adminConfig = GetAdminConfig(typeof(TModel));
            var adminService = _serviceProvider.GetService(adminConfig.AdminServiceType) as IAdminTreeService<TModel>;

            if (adminService == null)
            {
                throw new InvalidOperationException(string.Format(Resources.AdminServiceNotFoundInvalidOperation, typeof(TModel)));
            }
            var modelToUpdate = ((JObject)item).ToObject<TModel>(_serializer);
            return await adminService.UpdateTree(modelToUpdate);
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

        private async Task<IAdminResult> ExecuteAdminAction<TModel>(TModel entityObject, AdminAction adminAction) where TModel : class
        {
            var formActionDelegate = adminAction.FormActionExpression.Compile();
            var result = await (dynamic)formActionDelegate.DynamicInvoke(_serviceProvider, entityObject) as IAdminResult;
            return result;
        }

        private async Task<IAdminResult> ExecuteCustomFormSubmitAction<TModel>(TModel entityObject, CustomForm customForm) where TModel : class
        {
            var formActionDelegate = customForm.SubmitActionExpression.Compile();
            var result = await (dynamic)formActionDelegate.DynamicInvoke(_serviceProvider, entityObject) as IAdminResult;
            return result;
        }
    }
}
