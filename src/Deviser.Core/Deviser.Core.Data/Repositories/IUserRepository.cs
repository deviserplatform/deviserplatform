using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUser(Guid userId);
        User GetUser(string userName);

    }
}