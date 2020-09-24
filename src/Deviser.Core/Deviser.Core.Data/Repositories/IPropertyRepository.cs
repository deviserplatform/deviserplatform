using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IPropertyRepository
    {
        Property CreateProperty(Property dbProperty);
        List<Property> GetProperties();
        Property GetProperty(Guid propertyId);
        bool IsPropertyExist(string propertyName);
        Property UpdateProperty(Property dbProperty);
    }
}