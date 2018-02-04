using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Deviser.Core.Data.Extensions
{
    public static class DatabaseFacadeExtensions
    {
        public static bool Exists(this DatabaseFacade source)
        {
            return source.GetService<IRelationalDatabaseCreator>().Exists();
        }
    }
}
