using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Admin
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddDeviserAdmin<TAdminConfigurator, TContext>(this IServiceCollection serviceCollection)
            where TAdminConfigurator : class, IAdminConfigurator
            where TContext : DbContext
        {
           
            throw new NotImplementedException();
        }
    }
}
