using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using Deviser.Core.Common;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileProviders;

//using Microsoft.Extensions.PlatformAbstractions;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("*", Attributes = ContentEditsAttribute)]
    public class ContentEditViewHelper : DeviserTagHelper
    {
        private const string ContentEditsAttribute = "dev-content-edits";

        private readonly INavigation _navigation;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IScopeService _scopeService;

        [HtmlAttributeName(ContentEditsAttribute)]
        public string ContentEditViews { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ContentEditViewHelper(IHttpContextAccessor httpContextAccessor,
            INavigation navigation,
            IHtmlHelper htmlHelper,
            IScopeService scopeService,
            IWebHostEnvironment hostEnvironment)
             : base(httpContextAccessor)
        {
            _htmlHelper = htmlHelper;
            _navigation = navigation;
            _scopeService = scopeService;
            _hostingEnvironment = hostEnvironment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ContentEditViews))
            {
                output.Content.SetHtmlContent("Navigation style (dev-nav) cannot be null");
                return;
            }

            ((HtmlHelper)_htmlHelper).Contextualize(ViewContext);

            var contentResult = GetAllEditViews();
            output.Content.SetHtmlContent(contentResult);
            //output.PostContent.Append("MenuStyle: " + MenuStyle);
        }

        private string GetAllEditViews()
        {
            //try
            //{
                
                
            //    //var deviserWebAssembly = Assembly.Load(new AssemblyName("Deviser.Core.Library"));
            //    //    //System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies.First(a => a.EntryPoint != null);//Globals.EntryPointAssembly;
            //    //var manifestEmbeddedProvider = new ManifestEmbeddedFileProvider(deviserWebAssembly);
            //}
            //catch (Exception ex)
            //{
            //    throw; 
            //}

            var fp = _hostingEnvironment.ContentRootFileProvider;
            StringBuilder sb = new StringBuilder();
            string editPath = string.Format(Globals.ContentTypesEditPath, _scopeService.PageContext.SelectedTheme);
            string editViewDir = Path.Combine(_hostingEnvironment.ContentRootPath, Path.Combine(editPath.Replace("~/", "").Split('/')));
            DirectoryInfo dir = new DirectoryInfo(editViewDir);
            foreach (var file in dir.GetFiles())
            {
                if (file != null)
                {
                    var htmlContent = _htmlHelper.Partial(Path.Combine(editPath, file.Name));
                    var contentResult = GetString(htmlContent);
                    sb.Append(contentResult);
                }
            }
            return sb.ToString();
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }


}
