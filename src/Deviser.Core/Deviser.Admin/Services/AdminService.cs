using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Admin.Services
{
    public class AdminService<TModel, TEntity> : IAdminService<TModel>
        where TModel : class
        where TEntity : class
    {
        private readonly IMapper _modelMapper;
        private readonly IAdminService<TEntity> _adminServiceEntity;
        public AdminService(DbContext dbContext, IQueryable<TEntity> baseQuery, IMapper modelMapper)
        {
            _adminServiceEntity = new AdminServiceEntity<TEntity>(dbContext, baseQuery);
            _modelMapper = modelMapper;
        }

        public virtual async Task<PagedResult<TModel>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var entityResult = await _adminServiceEntity.GetAll(pageNo, pageSize, orderByProperties, filter);
            var result = new PagedResult<TModel>(_modelMapper.Map<ICollection<TModel>>(entityResult.Data), pageNo, pageSize, orderByProperties);
            return await Task.FromResult(result);
        }

        public virtual async Task<TModel> GetItem(string itemId)
        {
            var entityResult = await _adminServiceEntity.GetItem(itemId);
            var result = _modelMapper.Map<TModel>(entityResult);
            return await Task.FromResult(result);
        }

        public virtual async Task<IFormResult<TModel>> CreateItem(TModel item)
        {
            var entity = _modelMapper.Map<TEntity>(item);
            var entityResult = await _adminServiceEntity.CreateItem(entity);
            var result = new FormResult<TModel>(_modelMapper.Map<TModel>(entityResult.Value))
            {
                IsSucceeded = entityResult.IsSucceeded,
                SuccessMessage = entityResult.SuccessMessage,
                ErrorMessage = entityResult.ErrorMessage
            }; 
            return await Task.FromResult(result);
        }

        public virtual async Task<IFormResult<TModel>> UpdateItem(TModel item)
        {
            var entity = _modelMapper.Map<TEntity>(item);
            var entityResult = await _adminServiceEntity.UpdateItem(entity);
            var result = new FormResult<TModel>(_modelMapper.Map<TModel>(entityResult.Value))
            {
                IsSucceeded = entityResult.IsSucceeded,
                SuccessMessage = entityResult.SuccessMessage,
                ErrorMessage = entityResult.ErrorMessage
            };
            return await Task.FromResult(result);
        }

        public virtual async Task<IAdminResult<TModel>> DeleteItem(string itemId)
        {
            var entityResult = await _adminServiceEntity.DeleteItem(itemId);
            var result = new AdminResult<TModel>(_modelMapper.Map<TModel>(entityResult.Value))
            {
                IsSucceeded = entityResult.IsSucceeded,
                SuccessMessage = entityResult.SuccessMessage,
                ErrorMessage = entityResult.ErrorMessage
            };
            return await Task.FromResult(result);
        }
    }
}
