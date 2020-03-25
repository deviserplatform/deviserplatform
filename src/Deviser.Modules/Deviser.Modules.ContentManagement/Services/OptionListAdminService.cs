using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
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

        public async Task<PagedResult<OptionList>> GetAll(int pageNo, int pageSize, string orderByProperties)
        {
            var optionLists = _optionListRepository.GetOptionLists();
            var skip = (pageNo - 1) * pageSize;
            var total = optionLists.Count;
            var paging = optionLists.Skip(skip).Take(pageSize);
            var result = new PagedResult<OptionList>(paging, pageNo, pageSize, total);
            return await Task.FromResult(result);
        }

        public async Task<OptionList> GetItem(string itemId)
        {
            var result = _optionListRepository.GetOptionList(Guid.Parse(itemId));
            ParseResult(result);
            return await Task.FromResult(result);
        }

        public async Task<FormResult<OptionList>> CreateItem(OptionList item)
        {
            ParseProperty(item);
            var resultOptionList = _optionListRepository.CreateOptionList(item);
            ParseResult(resultOptionList);
            var result = new FormResult<OptionList>(resultOptionList);
            return await Task.FromResult(result);
        }

        public async Task<FormResult<OptionList>> UpdateItem(OptionList item)
        {
            ParseProperty(item);
            var resultOptionList = _optionListRepository.UpdateOptionList(item);
            ParseResult(resultOptionList);
            var result = new FormResult<OptionList>(resultOptionList);
            return await Task.FromResult(result);
        }

        public async Task<OptionList> DeleteItem(string itemId)
        {
            var optionList = _optionListRepository.GetOptionList(Guid.Parse(itemId));
            if (optionList == null)
            {
                return await Task.FromResult<OptionList>(null);
            }

            optionList.IsActive = false;
            var result = _optionListRepository.UpdateOptionList(optionList);
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
