using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Installation.Contexts
{
    public class PostgreSqlDbContext : DeviserDbContext
    {
        public PostgreSqlDbContext(DbContextOptions<DeviserDbContext> options)
            : base(options)
        {

        }
    }
}
