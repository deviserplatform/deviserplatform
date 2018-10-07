using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin
{

    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Company { get; set; }

        public string Street { get; set; }
        public string HouseNr { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
    public class TestClass
    {
        public class AdminConfigurator : IAdminConfigurator
        {
            public void ConfigureAdmin(IAdminSite adminSite)
            {
                //Register Student model, fields are automatically identified
                adminSite.Register<Student>();


                //Register Student model by specifying the fields to be included with custom configuration
                adminSite.Register<Student>(config =>
                {
                    config.FieldConfig
                    .AddField(s => s.FirstName).AddInlineField(s => s.LastName)
                    .AddField(s => s.BirthDate)
                    .AddField(s => s.Company)
                    .AddField(s => s.Street).AddInlineField(s => s.HouseNr)
                    .AddField(s => s.City).AddInlineField(s => s.PostalCode);
                });

                //Register Student model with custom configurations                
                adminSite.Register<Student>(config =>
                {
                    config.FieldConfig
                    .RemoveField(s => s.Company);
                });
                
                //Register Student model with custom configurations in fieldset              
                adminSite.Register<Student>(config =>
                {
                    config.FieldSetConfig
                    .AddFieldSet("General", fieldconfig =>
                        fieldconfig
                        .AddField(s => s.FirstName).AddInlineField(s => s.LastName)
                    )
                    .AddFieldSet("Biography", fieldconfig =>
                        fieldconfig
                        .AddField(s => s.BirthDate)
                        .AddField(s => s.Company)
                    )
                    .AddFieldSet("Address", fieldconfig =>
                        fieldconfig
                        .AddField(s => s.Street).AddInlineField(s => s.HouseNr)
                        .AddField(s => s.City).AddInlineField(s => s.PostalCode)
                    );
                });

                adminSite.Register<Student>(config =>
                {
                    config.ListConfig
                    .AddField(s => s.FirstName)
                    .AddField(s => s.LastName)
                    .AddField(s => s.BirthDate);


                });
            }
        }
    }
}
