using Microsoft.EntityFrameworkCore;

namespace Deviser.Admin
{
    public interface IAdminConfigurator
    {
        void ConfigureAdmin(IAdminBuilder adminBuilder);
    }

    public interface IAdminConfigurator<TContext> : IAdminConfigurator
        where TContext : DbContext
    {
        //void ConfigureAdmin(IAdminSite adminSite);
            
    }
}