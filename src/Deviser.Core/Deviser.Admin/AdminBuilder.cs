using AutoMapper;
using Deviser.Admin.Builders;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Admin
{
    public class AdminBuilder : IAdminBuilder
    {
        private readonly IAdminSite _adminSite;
        
        public AdminBuilder(IAdminSite adminSite)
        {
            _adminSite = adminSite;
        }

        public MapperConfiguration MapperConfiguration { get; set; }

        public AdminBuilder Register<TEntity>(Action<FormBuilder<TEntity>> formBuilderAction = null, AdminType adminConfigType = AdminType.Entity)
            where TEntity : class
        {
            var adminConfig = new AdminConfig<TEntity>();
            var hasConfiguration = formBuilderAction != null;

            _adminSite.Mapper = MapperConfiguration?.CreateMapper();

            formBuilderAction?.Invoke(new FormBuilder<TEntity>(adminConfig));

            _adminSite.Build(adminConfig, hasConfiguration);

            return this;
        }
    }
}
