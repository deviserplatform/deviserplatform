﻿using Microsoft.EntityFrameworkCore;

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
