using Deviser.Admin.Builders;
using Deviser.Admin.Config;
using System;

namespace Deviser.Admin
{
    public interface IAdminBuilder
    {
        AdminBuilder Register<TEntity>(Action<FormBuilder<TEntity>> formBuilderAction = null, AdminType adminConfigType = AdminType.Entity) where TEntity : class;
    }
}