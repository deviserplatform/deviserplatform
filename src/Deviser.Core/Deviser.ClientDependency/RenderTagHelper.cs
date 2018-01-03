using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;

namespace Deviser.ClientDependency
{
    [HtmlTargetElement("render", Attributes = DependencyTypeAttributeName)]
    [HtmlTargetElement("render", Attributes = ScriptLocationAttributeName)]
    public class RenderTagHelper : UrlResolutionTagHelper
    {
        private const string DependencyTypeAttributeName = "type";
        private const string ScriptLocationAttributeName = "location";

        private IHttpContextAccessor httpContextAccessor;
        private IHostingEnvironment hostingEnvironment;
        private IMemoryCache cache;

        [HtmlAttributeName(DependencyTypeAttributeName)]
        public DependencyType DependencyType { get; set; }

        [HtmlAttributeName(ScriptLocationAttributeName)]
        public ScriptLocation ScriptLocation { get; set; }


        public override int Order
        {
            get
            {
                return -1000;
            }
        }

        public RenderTagHelper(IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IMemoryCache cache,
            IUrlHelperFactory urlHelperFactory,
            HtmlEncoder htmlEncoder)
            :base(urlHelperFactory, htmlEncoder)
        {
            this.cache = cache;
            this.httpContextAccessor = httpContextAccessor;
            this.hostingEnvironment = hostingEnvironment;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            try
            {
                output.TagName = null;
                var dependencyLoader = DependencyManager.GetLoader(httpContextAccessor.HttpContext);
                var fileVersionProvider = new FileVersionProvider(
                    hostingEnvironment.WebRootFileProvider,
                    cache,
                    ViewContext.HttpContext.Request.PathBase);
                //var sb = new StringBuilder();
                var dependencyFiles = dependencyLoader.DependencyFiles
                       .Where(df => df.DependencyType == DependencyType)
                       .OrderBy(df => df.Priority)
                       .ToList();
                if (DependencyType == DependencyType.Css)
                {
                    foreach (var file in dependencyFiles)
                    {
                        TagBuilder itemBuilder = new TagBuilder("link");
                        string resolvedPath;

                        if (file.FilePath.StartsWith("~"))
                        {
                            TryResolveUrl(file.FilePath, out resolvedPath);
                            //resolvedPath = fileVersionProvider.AddFileVersionToPath(resolvedPath);
                        }
                        else
                        {
                            resolvedPath = file.FilePath;
                        }
                        

                        itemBuilder.MergeAttributes(file.Attributes);
                        itemBuilder.Attributes.Add("href", resolvedPath);
                        itemBuilder.Attributes.Add("rel", "stylesheet");
                        output.Content.AppendHtml(itemBuilder);
                        output.Content.AppendHtml(Environment.NewLine);
                    }
                }
                else if (DependencyType == DependencyType.Script)
                {
                    var filteredFiles = dependencyFiles.Where(df => df.ScriptLocation == ScriptLocation).ToList();

                    foreach (var file in filteredFiles)
                    {
                        TagBuilder itemBuilder = new TagBuilder("script");
                        string resolvedPath;

                        if (file.FilePath.StartsWith("~"))
                        {
                            TryResolveUrl(file.FilePath, out resolvedPath);
                            //resolvedPath = fileVersionProvider.AddFileVersionToPath(resolvedPath);
                        }
                        else
                        {
                            resolvedPath = file.FilePath;
                        }

                        itemBuilder.MergeAttributes(file.Attributes);
                        itemBuilder.Attributes.Add("src", resolvedPath);
                        output.Content.AppendHtml(itemBuilder);
                        output.Content.AppendHtml(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
