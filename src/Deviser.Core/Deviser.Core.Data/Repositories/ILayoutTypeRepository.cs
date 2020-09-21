using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface ILayoutTypeRepository
    {
        LayoutType CreateLayoutType(LayoutType dbLayoutType);
        List<LayoutType> GetLayoutTypes();
        LayoutType GetLayoutType(Guid layoutTypeId);
        LayoutType GetLayoutType(string layoutTypeName);
        LayoutType UpdateLayoutType(LayoutType dbLayoutType);

    }
}