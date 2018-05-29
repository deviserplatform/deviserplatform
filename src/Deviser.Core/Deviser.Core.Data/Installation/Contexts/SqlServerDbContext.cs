using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Installation.Contexts
{
    public class SqlServerDbContext : DeviserDbContext
    {
        public SqlServerDbContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}
