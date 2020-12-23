using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Detached;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Deviser.Admin.Services
{
    public class AdminServiceEntity<TEntity> : IAdminService<TEntity>
        where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly IQueryable<TEntity> _baseQuery;
        private readonly IKey _primaryKey;
        private readonly Type _entityClrType;
        private readonly IEnumerable<INavigation> _navigations;

        public AdminServiceEntity(DbContext dbContext, IQueryable<TEntity> baseQuery)
        {
            _dbContext = dbContext;
            _baseQuery = baseQuery;
            _entityClrType = typeof(TEntity);
            var entityType = _dbContext.Model.FindEntityType(_entityClrType);
            _primaryKey = entityType.FindPrimaryKey();
            _navigations = entityType.GetNavigations();
        }

        public virtual async Task<PagedResult<TEntity>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            // Determine the number of records to skip
            var skip = (pageNo - 1) * pageSize;
            // Get total number of records
            var total = _baseQuery.Count();
            var query = _baseQuery.Skip(skip).Take(pageSize);
            var dbResult = await query.ToListAsync();
            var pagedResult = new PagedResult<TEntity>(dbResult, pageNo, pageSize, orderByProperties);
            return await Task.FromResult(pagedResult);
        }

        public virtual async Task<TEntity> GetItem(string itemId)
        {
            var filterExpression = AdminRepository.CreatePrimaryKeyFilter(_primaryKey, new List<string> { itemId });
            var whereCallExpression = ExpressionHelper.GetWhereExpression(_entityClrType, _baseQuery.Expression, filterExpression);
            var query = _baseQuery.Provider.CreateQuery<TEntity>(whereCallExpression);
            var dbResult = await query.FirstOrDefaultAsync();
            return await Task.FromResult(dbResult);
        }

        public virtual async Task<IFormResult<TEntity>> CreateItem(TEntity item)
        {
            var dbSet = _dbContext.Set<TEntity>();

            //foreach (var navigation in _navigations)
            //{
            //    var navigationPropertyInfo = navigation.PropertyInfo;
            //    var navigationVal = navigationPropertyInfo.GetValue(item);
            //    if(navigationVal==null) continue;

            //    if (navigation.IsCollection)
            //    {
                    
            //    }
            //    else
            //    {
            //        var propertyType = navigationPropertyInfo.PropertyType;
            //        var entityType = _dbContext.Model.FindEntityType(propertyType);
            //        var pk = entityType.FindPrimaryKey();
            //        var pkPropInfo = pk.Properties.First().PropertyInfo; //Assuming the entity has only one primary key
            //        var id = pkPropInfo.GetValue(navigationVal, null); //item.Category.Id

            //        var targetPropInfo = entityType.GetNavigations().First().ForeignKey.Properties.First().PropertyInfo;
            //        targetPropInfo.SetValue(item, id, null); //item.CategoryId = item.Category.Id
            //        navigationPropertyInfo.SetValue(item, null, null); //item.Category = null;
            //    }
            //}

            var queryableData = await dbSet.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            if (queryableData.Entity == null)
                return new FormResult<TEntity>()
                {
                    IsSucceeded = false,
                    ErrorMessage = $"Unable to create the {_entityClrType.Name}"
                };

            var result = new FormResult<TEntity>(queryableData.Entity)
            {
                IsSucceeded = true,
                SuccessMessage = $"{_entityClrType.Name} has been created"
            };

            return await Task.FromResult(result);
        }

        public virtual async Task<IFormResult<TEntity>> UpdateItem(TEntity item)
        {
            var queryableData = _dbContext.UpdateGraph(item);
            await _dbContext.SaveChangesAsync();

            if (queryableData == null)
                return new FormResult<TEntity>()
                {
                    IsSucceeded = false,
                    ErrorMessage = $"Unable to update the {_entityClrType.Name}"
                };

            var keyProperty = _primaryKey.Properties.First();
            var id = keyProperty.PropertyInfo.GetValue(queryableData)?.ToString();
            var tag = await GetItem(id);
            var result = new FormResult<TEntity>(tag)
            {
                IsSucceeded = true,
                SuccessMessage = $"{_entityClrType.Name} has been updated"
            };
            return await Task.FromResult(result);
        }

        public virtual async Task<IAdminResult<TEntity>> DeleteItem(string itemId)
        {
            var itemToDelete = await GetItem(itemId);
            var dbSet = _dbContext.Set<TEntity>();
            var queryableData = dbSet.Remove(itemToDelete);
            await _dbContext.SaveChangesAsync();
            var result = new FormResult<TEntity>(queryableData.Entity)
            {
                IsSucceeded = true,
                SuccessMessage = $"{_entityClrType.Name} has been deleted"
            };
            return await Task.FromResult(result);
        }
    }
}
