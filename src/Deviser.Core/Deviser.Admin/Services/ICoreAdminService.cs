﻿using System;
using System.Threading.Tasks;
using Deviser.Admin.Config;

namespace Deviser.Admin.Services
{
    public interface ICoreAdminService
    {
        Type GetModelType(string strModelType);
        Type GetCustomFormModelType(string strModelType, string formName);
        IAdminConfig GetAdminConfig(string entity);

        Task<object> GetAllFor(Type modelType, int pageNo, int pageSize, string orderByProperties);
        Task<object> GetItemFor(Type modelType, string itemId);
        Task<object> CreateItemFor(Type modelType, object item);
        Task<object> UpdateItemFor(Type modelType, object item);
        Task<object> DeleteItemFor(Type modelType, string itemId);
        Task<object> ExecuteMainFormAction(Type modelType, string actionName, object entityObject);
        Task<object> ExecuteCustomFormAction(Type modelType, string formName, string actionName, object entityObject);
        Task<object> CustomFormSubmit(string strModel, string formName, object entityObject);
        Task<ValidationResult> ExecuteMainFormCustomValidation(Type modelType, string fieldName, object fieldObject);

        Task<ValidationResult> ExecuteChildFormCustomValidation(Type modelType, string formName, string fieldName,
            object fieldObject);

        Task<ValidationResult> ExecuteCustomFormCustomValidation(Type modelType, string formName, string fieldName,
            object fieldObject);


    }
}
