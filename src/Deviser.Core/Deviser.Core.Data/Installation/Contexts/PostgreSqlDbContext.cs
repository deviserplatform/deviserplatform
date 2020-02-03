using Microsoft.EntityFrameworkCore;

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
