using Microsoft.Extensions.DependencyInjection;
using System;

namespace Deviser.Core.Common.Internal
{
    public class InternalServiceProvider
    {
        private IServiceCollection _serviceCollection;
        
        public static InternalServiceProvider Instance { get; } = new InternalServiceProvider();

        public void BuildServiceProvider(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            ServiceProvider = _serviceCollection.BuildServiceProvider();
        }

        public IServiceProvider ServiceProvider { get; private set; }
    }
}
