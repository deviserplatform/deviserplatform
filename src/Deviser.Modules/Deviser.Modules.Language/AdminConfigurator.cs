using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deviser.Admin;
using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Multilingual;
using Deviser.Modules.Language.Services;
using Microsoft.Extensions.DependencyInjection;
using FieldType = Deviser.Admin.Config.FieldType;

namespace Deviser.Modules.Language
{
    public class AdminConfigurator : IAdminConfigurator
    {
        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<Core.Common.DomainTypes.Language, LanguageAdminService>(builder =>
            {
                var gridBuilder = builder.GridBuilder;
                var formBuilder = builder.FormBuilder;

                gridBuilder
                    .AddKeyField(r => r.CultureCode)
                    .AddField(r => r.EnglishName, option => option.DisplayName = "English Name")
                    .AddField(r => r.NativeName, option => option.DisplayName = "Native Name")
                    .AddField(r => r.IsActive, option =>
                    {
                        option.DisplayName = "Is Active";
                        option.IsTrue = "Active";
                        option.IsFalse = "In Active";
                    });

                gridBuilder.DisplayFieldAs(c => c.IsActive, LabelType.Badge, c => c.IsActiveBadgeClass);


                formBuilder
                    .AddField(l => l.EnglishName, option =>
                    {
                        option.DisplayName = "English Name";
                        option.ShowIn = FormMode.Update;
                        option.IsReadOnly = true;
                    })
                    .AddField(l => l.NativeName, option =>
                    {
                        option.DisplayName = "Native Name";
                        option.ShowIn = FormMode.Update;
                        option.IsReadOnly = true;
                    })
                    .AddField(l => l.CultureCode, option =>
                    {
                        option.DisplayName = "Culture Code";
                        option.ShowIn = FormMode.Update;
                        option.IsReadOnly = true;
                    })
                    .AddSelectField(l => l.SelectedLanguage, null, option =>
                    {
                        option.DisplayName = "Select Language";
                        option.ShowIn = FormMode.Create;
                    });

                formBuilder.AddField(l => l.IsActive);

                formBuilder.Property(l => l.IsActive).EnableOn(l => l.CultureCode != "en-US");

                formBuilder.Property(l => l.SelectedLanguage).HasLookup(
                    sp => sp.GetService<LanguageAdminService>().GetAllLanguages(),
                    language => language.CultureCode,
                    language => language.EnglishName);
            });
        }
    }
}
