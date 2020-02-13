using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.Installation.Contexts
{
    public class SqlServerDbContext : DeviserDbContext
    {
        public SqlServerDbContext(DbContextOptions<DeviserDbContext> options)
            : base(options)
        {

        }
    }
}
