using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Http;

namespace Deviser.ClientDependency
{
    [HtmlTargetElement("require", Attributes = DependencyTypeAttributeName)]
    public class RequireTagHelper : TagHelper
    {
        private const string DependencyTypeAttributeName = "type";
        private const string PathAttributeName = "path";

        private IHttpContextAccessor httpContextAccessor;

        [HtmlAttributeName(DependencyTypeAttributeName)]
        public DependencyType DependencyType { get; set; }

        public string Path { get; set; }

        public int Priority { get; set; }

        public override int Order
        {
            get
            {
                return -1000;
            }
        }

        public RequireTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (string.IsNullOrEmpty(Path))
            {
                throw new ArgumentNullException("Path", "Path must be provided");
            }

            try
            {
                var dependencyLoader = DependencyManager.GetLoader(httpContextAccessor.HttpContext);
                if (!dependencyLoader.DependencyFiles.Any(d => d.FilePath.ToLower() == Path.ToLower()))
                {
                    dependencyLoader.DependencyFiles.Add(new ClientDependency.DependencyFile
                    {
                        DependencyType = DependencyType,
                        FilePath = Path,
                        Priority = Priority > 0 ? Priority : Priority + 100,
                        Attributes = output.Attributes.ToDictionary(k => k.Name, v => v.Value)
                    });
                }
                output.Content.SetContent(null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
