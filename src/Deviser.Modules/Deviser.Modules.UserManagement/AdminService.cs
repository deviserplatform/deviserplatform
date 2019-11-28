using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Modules.UserManagement
{
    public class AdminService : IAdminService<User>
    {
        public async Task<User> CreateItem(User item)
        {
            throw new NotImplementedException();
        }

        public async Task<User> DeleteItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<User>> GetAll(int pageNo, int pageSize, string orderByProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetItem(string itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateItem(User item)
        {
            throw new NotImplementedException();
        }
    }
}
