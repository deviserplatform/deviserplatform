using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Installation.Contexts
{
    public class MySqlDbContext : DeviserDbContext
    {
        public MySqlDbContext(DbContextOptions<DeviserDbContext> options)
            : base(options)
        {

        }
    }
}
