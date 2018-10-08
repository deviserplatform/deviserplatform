using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Deviser.Admin.Config
{
    public class AdminSite : IAdminSite
    {
        private readonly DbContext _dbContext;
        private readonly Type _dbContextType;

        public AdminSite(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("Constructor paramater dbContent cannot be null");

            _dbContext = dbContext;
            _dbContextType = _dbContext.GetType();
        }

        public void Register<TEntity>(Action<AdminConfig<TEntity>> adminConfigAction = null) where TEntity : class
        {
            var entityClrType = typeof(TEntity);
            var entityType = _dbContext.Model.FindEntityType(entityClrType);

            if (entityType == null)
                throw new InvalidOperationException($"The entity type {entityClrType} cannot be found on this context {_dbContext}");

            var adminConfig = new AdminConfig<TEntity>();



            if (adminConfigAction == null)
            {
                //Register by default settings and fields
                PopulateFields(entityClrType, entityType, adminConfig);
            }
            else
            {
                adminConfigAction(adminConfig);
                PopulateEntityConfig(entityType, adminConfig);

                bool hasExlcludeFields = adminConfig?.FieldConfig?.ExcludedFields?.Count > 0;

                if (hasExlcludeFields)
                {
                    PopulateFields(entityClrType, entityType, adminConfig, adminConfig.FieldConfig.ExcludedFields);
                }
                else if(adminConfig?.FieldConfig?.Fields?.Count > 0)
                {
                    foreach (var fieldRow in adminConfig.FieldConfig.Fields)
                    {
                        foreach (var field in fieldRow)
                        {
                            PopulateFieldOptions(field, entityType);
                        }
                    }
                }
                else if (adminConfig?.FieldSetConfig?.FieldSets?.Count > 0)
                {
                    foreach (var fieldSet in adminConfig.FieldSetConfig.FieldSets)
                    {
                        foreach (var fieldRow in fieldSet.Fields)
                        {
                            foreach (var field in fieldRow)
                            {
                                PopulateFieldOptions(field, entityType);
                            }
                        }
                    }
                }

                if (adminConfig?.ListConfig?.Fields?.Count > 0)
                {
                    foreach (var field in adminConfig.ListConfig.Fields)
                    {
                        PopulateFieldOptions(field, entityType);
                    }
                }
            }


           
        }

        private void PopulateFields<TEntity>(Type entityClrType, IEntityType entityType, AdminConfig<TEntity> adminConfig, List<Field> excludeField = null) where TEntity : class
        {
            
            var properties = entityType.GetProperties();
            foreach (var prop in properties)
            {
                bool isExclude = (excludeField != null && excludeField.Any(f => f.FieldClrType == prop.ClrType));

                if (!isExclude)
                {
                    adminConfig.FieldConfig.Fields.Add(new List<Field>
                    {
                        new Field
                        {
                            FieldExpression = GetFieldExpression(entityClrType, prop)
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Returns the FieldExpression for the given parameters. E.g. e => e.Name
        /// </summary>
        /// <param name="entityClrType"></param>
        /// <param name="efProperty"></param>
        /// <returns>Expression<Func<TEntity, TKey>> as LambdaExpression</returns>
        private LambdaExpression GetFieldExpression(Type entityClrType, IProperty efProperty)
        {
            ParameterExpression param = Expression.Parameter(entityClrType);
            MemberExpression memberExpression = Expression.Property(param, efProperty.PropertyInfo);
            //var keyExpression = Expression.Lambda<Func<TEntity, TKey>>(memberExpression, param);
            var fieldExpression = Expression.Lambda(memberExpression, param);
            return fieldExpression;
        }

        private void PopulateEntityConfig<TEntity>(IEntityType entityType, AdminConfig<TEntity> adminConfig) where TEntity : class
        {
            adminConfig.EntityConfig.DbContextType = _dbContext.GetType();
            adminConfig.EntityConfig.PrimaryKey = entityType.FindPrimaryKey();
            adminConfig.EntityConfig.ForeignKeys = entityType.GetForeignKeys();
            adminConfig.EntityConfig.Navigations = entityType.GetNavigations();
        }

        private void PopulateFieldOptions(Field field, IEntityType entityType)
        {
            var attributes = field.FieldClrType.GetTypeInfo().GetCustomAttributes();
            var efProperty = entityType.GetProperties().FirstOrDefault(p => p.ClrType == field.FieldClrType);
            field.FieldOption = new FieldOption();


            //var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            var displayFormatAttribute = attributes.OfType<DisplayFormatAttribute>().FirstOrDefault();
            var displayNameAttribute = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

            //TODO: Localization in future
            //IStringLocalizer localizer = null;
            //if (_stringLocalizerFactory != null && _localizationOptions.DataAnnotationLocalizerProvider != null)
            //{
            //    localizer = _localizationOptions.DataAnnotationLocalizerProvider(containerType, _stringLocalizerFactory);
            //}


            // DisplayName
            // DisplayAttribute has precedence over DisplayNameAttribute.

            if (string.IsNullOrEmpty(field.FieldOption.DisplayName))
            {
                if (displayAttribute?.GetName() != null)
                {
                    field.FieldOption.DisplayName = displayAttribute.GetName();
                }
                else if (displayNameAttribute != null)
                {
                    //if (localizer != null &&
                    //    !string.IsNullOrEmpty(displayNameAttribute.DisplayName))
                    //{
                    //    displayMetadata.DisplayName = () => localizer[displayNameAttribute.DisplayName];
                    //}
                    //else
                    //{
                    field.FieldOption.DisplayName = displayNameAttribute.DisplayName;
                    //}
                }
                else
                {
                    field.FieldOption.DisplayName = field.FieldName;
                }
            }


            if (string.IsNullOrEmpty(field.FieldOption.Format) && displayFormatAttribute != null)
            {
                field.FieldOption.Format = displayFormatAttribute.DataFormatString;
            }

            if (string.IsNullOrEmpty(field.FieldOption.Description) && displayAttribute != null)
            {
                //if (localizer != null &&
                //    !string.IsNullOrEmpty(displayAttribute.Description) &&
                //    displayAttribute.ResourceType == null)
                //{
                //    displayMetadata.Description = () => localizer[displayAttribute.Description];
                //}
                //else
                //{
                field.FieldOption.Description = displayAttribute.GetDescription();
                //}
            }

            // NullDisplayText
            if (string.IsNullOrEmpty(field.FieldOption.NullDisplayText) && displayFormatAttribute != null)
            {
                field.FieldOption.NullDisplayText = displayFormatAttribute.NullDisplayText;
            }

            if (field.FieldOption.MaxLength == 0)
            {
                field.FieldOption.MaxLength = efProperty.GetMaxLength() ?? 0;
            }

            field.FieldOption.IsRequired = !efProperty.IsColumnNullable();
        }

        private IKey GetPrimaryKey(Type clrType)
        {
            var entityType = _dbContext.Model.FindEntityType(clrType);
            var key = entityType.FindPrimaryKey();
            return key;
        }
    }
}
