using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Extensions;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Deviser.Modules.ContentManagement.Services
{
    public class OptionListAdminService : IAdminService<OptionList>
    {
        private readonly ILogger<OptionListAdminService> _logger;
        private readonly IOptionListRepository _optionListRepository;

        public OptionListAdminService(ILogger<OptionListAdminService> logger,
            IOptionListRepository optionListRepository)
        {
            _logger = logger;
            _optionListRepository = optionListRepository;
        }

        public async Task<PagedResult<OptionList>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var optionLists = _optionListRepository.GetOptionLists();
            if (filter != null)
            {
                optionLists = optionLists.ApplyFilter(filter).ToList();
            }
            var result = new PagedResult<OptionList>(optionLists, pageNo, pageSize, orderByProperties);
            return await Task.FromResult(result);
        }

        public async Task<OptionList> GetItem(string itemId)
        {
            var result = _optionListRepository.GetOptionList(Guid.Parse(itemId));
            ParseResult(result);
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<OptionList>> CreateItem(OptionList item)
        {
            ParseProperty(item);
            var resultOptionList = _optionListRepository.CreateOptionList(item);
            ParseResult(resultOptionList);
            if (resultOptionList == null)
                return new FormResult<OptionList>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to create the OptionList"
                };

            var result = new FormResult<OptionList>(resultOptionList)
            {
                IsSucceeded = true,
                SuccessMessage = "OptionList has been created successfully"
            };
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<OptionList>> UpdateItem(OptionList item)
        {
            ParseProperty(item);
            var resultOptionList = _optionListRepository.UpdateOptionList(item);
            ParseResult(resultOptionList);
            if (resultOptionList == null)
                return new FormResult<OptionList>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to save the OptionList"
                };

            var result = new FormResult<OptionList>(resultOptionList)
            {
                IsSucceeded = true,
                SuccessMessage = "OptionList has been saved successfully"
            };
            return await Task.FromResult(result);
        }

        public async Task<IAdminResult<OptionList>> DeleteItem(string itemId)
        {
            var optionList = _optionListRepository.GetOptionList(Guid.Parse(itemId));
            if (optionList == null)
            {
                return await Task.FromResult<AdminResult<OptionList>>(null);
            }

            optionList.IsActive = false;
            optionList = _optionListRepository.UpdateOptionList(optionList);
            if (optionList == null)
                return new FormResult<OptionList>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to delete the OptionList"
                };

            var result = new AdminResult<OptionList>(optionList)
            {
                IsSucceeded = true,
                SuccessMessage = "OptionList has been deleted successfully"
            };
            return await Task.FromResult(result);
        }

        public async Task<ValidationResult> ValidatePropertyName(string propertyName)
        {
            var result = _optionListRepository.IsPropertyExist(propertyName) ? ValidationResult.Failed(new ValidationError() { Code = "OptionList available!", Description = "OptionList already exist" }) : ValidationResult.Success;
            return await Task.FromResult(result);
        }

        private static void ParseResult(OptionList result)
        {
            //if (!string.IsNullOrEmpty(result.DefaultValue))
            //{
            //    result.DefaultValuePropertyOption =
            //        result.OptionList?.List?.FirstOrDefault(li => li.Id == Guid.Parse(result.DefaultValue));
            //}
        }

        private static void ParseProperty(OptionList item)
        {
            //item.OptionListId = item.OptionList?.Id;
            //item.OptionList = null;
            //if (item.DefaultValuePropertyOption != null)
            //{
            //    item.DefaultValue = item.DefaultValuePropertyOption.Id.ToString();
            //}
        }
    }
}
