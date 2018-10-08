using Microsoft.EntityFrameworkCore;

namespace Deviser.Admin
{
    public interface IAdminConfigurator<TContext>
        where TContext : DbContext
    {

        void ConfigureAdmin(IAdminSite adminSite);
            
    }
}