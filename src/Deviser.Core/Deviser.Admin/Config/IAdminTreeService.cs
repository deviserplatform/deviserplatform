﻿using System.Collections.Generic;
using Deviser.Admin.Data;
using System.Threading.Tasks;


namespace Deviser.Admin.Config
{
    public interface IAdminTreeService<TModel> //: IAdminService
        where TModel : class
    {
        Task<TModel> GetTree();
        Task<TModel> GetItem(string itemId);
        Task<IFormResult<TModel>> CreateItem(TModel item);
        Task<IFormResult<TModel>> UpdateItem(TModel item);
        Task<IFormResult<TModel>> UpdateTree(TModel item);
        Task<IAdminResult<TModel>> DeleteItem(string itemId);
    }
}
