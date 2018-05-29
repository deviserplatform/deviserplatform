using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Installation.Contexts
{
    public class SqlLiteDbContext : DeviserDbContext
    {
        public SqlLiteDbContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}
