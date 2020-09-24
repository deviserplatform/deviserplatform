using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.Repositories
{
    public interface IInstallationProvider
    {
        //bool IsPlatformInstalled();
        //bool IsDatabaseExist();

        bool IsPlatformInstalled { get; }

        bool IsDatabaseExist { get; }

        Task InstallPlatform(InstallModel installModel);
        void InsertData(DbContextOptions<DeviserDbContext> dbOption);
        string GetConnectionString(InstallModel model);
        DbContextOptionsBuilder GetDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder, string moduleAssembly = null);
        DbContextOptionsBuilder<TContext> GetDbContextOptionsBuilder<TContext>(DbContextOptionsBuilder optionsBuilder, string moduleAssembly = null)
            where TContext : DbContext;

    }
}