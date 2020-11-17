using Deviser.Core.Common;
using Deviser.Core.Common.Hubs;
using Deviser.Core.Common.Internal;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Deviser.Web.Builder
{
    public static class DeviserPlatformBuilderExtension
    {
        static readonly string AllowOrigin = "AllowOrigin";

        public static IApplicationBuilder UseDeviserPlatform(this IApplicationBuilder app, Action<IEndpointRouteBuilder> configure = null)
        {
            var serviceProvider = app.ApplicationServices;
            var env = serviceProvider.GetService<IWebHostEnvironment>();
            var isDevelopment = env.IsEnvironment(Globals.DeviserDevelopmentEnvironment);
            



            if (isDevelopment)
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
                requestLocalizationOptions.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(
                    async context =>
                    {
                        var isSiteMultilingual = languageRepository.IsMultilingual();
                        if (!isSiteMultilingual) return null;

                        var reqParts = context.Request.Path.ToString().Split('/');

                        if (reqParts.Length <= 1) return null;

                        var permalink = reqParts[1];
                        var match = Regex.Match(permalink, @"[a-z]{2}-[a-z]{2}", RegexOptions.IgnoreCase);
                        if (!match.Success) return null;

                        var requestCulture = new CultureInfo(match.Value);
                        var result = new ProviderCultureResult(requestCulture.Name);
                        return await Task.FromResult(result);

                    }));
                app.UseRequestLocalization(requestLocalizationOptions);
            }
            else
            {
                var supportedCultures = new[] { "en-US"};
                var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);

                app.UseRequestLocalization(localizationOptions);
            }

            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //        new CultureInfo(enUSCulture),
            //        new CultureInfo("fr")
            //    };

            //    options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSCulture);
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;

            //    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
            //    {
            //        // My custom request culture logic
            //        return new ProviderCultureResult("en");
            //    }));
            //});

            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            if (isDevelopment)
            {
                app.UseCors(AllowOrigin);
            }
            else
            {
                app.UseCors("default");
            }

            //Deviser Specific
            app.UsePageContext();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UsePageAuthorization();
            //app.Use(async (context, next) =>
            //{
            //    var cultureQuery = context.Request.Query["culture"];
            //    if (!string.IsNullOrWhiteSpace(cultureQuery))
            //    {
            //        var culture = new CultureInfo(cultureQuery);

            //        CultureInfo.CurrentCulture = culture;
            //        CultureInfo.CurrentUICulture = culture;
            //    }
            //    // Call the next delegate/middleware in the pipeline
            //    await next();
            //});



            app.UseSession();

            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ApplicationHub>("/appHub");

                endpoints.MapControllerRoute(name: Globals.moduleRoute,
                    pattern: "modules/{area:exists}/{controller=Home}/{action=Index}")
                    .RequireCors(AllowOrigin);

                endpoints.MapControllerRoute(name: "default",
                    pattern: "{controller=Page}/{action=Index}/{id?}")
                    .RequireCors(AllowOrigin);

                endpoints.MapControllerRoute(name: "CmsRoute",
                    pattern: "{**permalink}",
                    defaults: new { controller = "Page", action = "Index" },
                    constraints: new { permalink = InternalServiceProvider.Instance.ServiceProvider.GetService<IRouteConstraint>() })
                    .RequireCors(AllowOrigin);

                configure?.Invoke(endpoints);
            });
        }

        private static string GetPermalink(HttpContext httpContext)
        {
            var routeData = httpContext.GetRouteData();
            //permalink in the url has first preference
            var permalink = (routeData.Values["permalink"] != null) ? routeData.Values["permalink"].ToString() : "";
            if (string.IsNullOrEmpty(permalink))
            {
                //if permalink is null, check for querystring
                permalink = httpContext.Request.Query["permalink"].ToString();
            }
            return permalink;
        }
    }
}
