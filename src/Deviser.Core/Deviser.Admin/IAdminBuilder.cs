using Deviser.Admin.Builders;
using System;

namespace Deviser.Admin
{
    public interface IAdminBuilder
    {
        AdminBuilder Register<TEntity>(Action<FormBuilder<TEntity>> formBuilderAction = null) where TEntity : class;
    }
}