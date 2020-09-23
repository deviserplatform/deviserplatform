using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface ILayoutRepository
    {
        Layout CreateLayout(Layout layout);
        List<Layout> GetLayouts();
        List<Layout> GetDeletedLayouts();
        Layout GetLayout(Guid layoutId);
        Layout UpdateLayout(Layout layout);
        bool DeleteLayout(Guid layoutId);
    }
}