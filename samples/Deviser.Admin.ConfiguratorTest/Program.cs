using Deviser.Admin.ConfiguratorTest.Models;
using Deviser.Admin.Extensions;
using System;

namespace Deviser.Admin.ConfiguratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class AdminConfigurator : IAdminConfigurator<DeviserWIContext>
    {
        public void ConfigureAdmin(IAdminSite adminSite)
        {
            //adminSite.SiteName = "TestAdmin";

            ////Register Student model by specifying the fields to be included with custom configuration
            //adminSite.Register<User>(config =>
            //{
            //    config.FieldConfig
            //    .AddField(s => s.FirstName).AddInlineField(s => s.LastName)
            //    .AddField(s => s.PhoneNumber)
            //    .AddField(s => s.UserRole);
            //});
        }


        public void ConfigureAdmin(IAdminBuilder adminBuilder)
        {
            adminBuilder.Register<User>(form =>
            {
                form.Fields
                .AddField(u => u.FirstName).AddInlineField(u => u.LastName);

                form.FieldSetBuilder
                .AddFieldSet("General", fieldBuilder =>
                                        fieldBuilder.AddField(s => s.FirstName).AddInlineField(s => s.LastName)
                                    );
            });

        }
    }
}
