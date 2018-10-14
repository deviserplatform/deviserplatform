using Microsoft.EntityFrameworkCore;

namespace Deviser.Admin
{
    public interface IAdminConfigurator
    {
        void ConfigureAdmin(IAdminSite adminSite);
    }

    public interface IAdminConfigurator<TContext> : IAdminConfigurator
        where TContext : DbContext
    {
        //void ConfigureAdmin(IAdminSite adminSite);
            
    }
}