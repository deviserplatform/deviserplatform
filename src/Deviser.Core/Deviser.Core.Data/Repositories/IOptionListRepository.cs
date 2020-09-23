using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IOptionListRepository
    {
        OptionList CreateOptionList(OptionList dbOptionList);
        List<OptionList> GetOptionLists();
        OptionList GetOptionList(Guid optionListId);
        OptionList GetOptionList(string listName);
        OptionList UpdateOptionList(OptionList dbContentType);
        bool IsPropertyExist(string propertyName);
    }
}