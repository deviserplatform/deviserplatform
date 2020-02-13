using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.Installation.Contexts
{
    public class SqlLiteDbContext : DeviserDbContext
    {
        public SqlLiteDbContext(DbContextOptions<DeviserDbContext> options)
            : base(options)
        {

        }
    }
}
