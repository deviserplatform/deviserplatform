using Deviser.Core.Common;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Hubs;
using Deviser.Core.Library.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Linq;
using Deviser.Core.Common.Internal;
using Microsoft.AspNetCore.Localization;

namespace Deviser.Web.Builder
{
    public static class DeviserPlatformBuilderExtension
    {
        public static IApplicationBuilder UseDeviserPlatform(this IApplicationBuilder app, Action<IEndpointRouteBuilder> configure = null)
        {
            var serviceProvider = app.ApplicationServices;
            var env = serviceProvider.GetService<IWebHostEnvironment>();

            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //var defaultRequestCulture = new RequestCulture(new CultureInfo(enUSCulture));

            var installationProvider = serviceProvider.GetService<IInstallationProvider>();

            if (installationProvider.IsPlatformInstalled)
            {
                var languageRepository = InternalServiceProvider.Instance.ServiceProvider.GetService<ILanguageRepository>();
                var activeLanguages = languageRepository.GetActiveLanguages();
                var supportedCultures = activeLanguages.Select(al => new CultureInfo(al.CultureCode)).ToArray();

                var siteSettingRepository = InternalServiceProvider.Instance.ServiceProvider.GetService<ISiteSettingRepository>();
                var siteSettings = siteSettingRepository.GetSettingsAsDictionary();
                var defaultRequestCulture = new RequestCulture(new CultureInfo(siteSettings["SiteLanguage"]));
                var requestLocalizationOptions = new RequestLocalizationOptions
                {
                    DefaultRequestCulture = defaultRequestCulture,
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                };
                app.UseRequestLocalization(requestLocalizationOptions);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                var cultureQuery = context.Request.Query["culture"];
                if (!string.IsNullOrWhiteSpace(cultureQuery))
                {
                    var culture = new CultureInfo(cultureQuery);

                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
                // Call the next delegate/middleware in the pipeline
                await next();
            });

            app.UseSession();

            //Deviser Specific
            app.UsePageContext();

            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ApplicationHub>("/appHub");

                endpoints.MapControllerRoute(name: Globals.moduleRoute,
                    pattern: "modules/{area:exists}/{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(name: "default",
                    pattern: "{controller=Page}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(name: "CmsRoute",
                    pattern: "{**permalink}",
                    defaults: new { controller = "Page", action = "Index" },
                    constraints: new { permalink = InternalServiceProvider.Instance.ServiceProvider.GetService<IRouteConstraint>() });

                configure?.Invoke(endpoints);
            });
        }
    }
}
