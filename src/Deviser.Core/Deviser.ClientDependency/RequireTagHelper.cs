using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;

namespace Deviser.ClientDependency
{    
    public class RequireTagHelper : TagHelper
    {
        private static readonly char[] NameSeparator = new[] { ',' };
        
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// A comma separated list of environment names in which the content should be rendered.
        /// If the current environment is also in the <see cref="Exclude"/> list, the content will not be rendered.
        /// </summary>
        /// <remarks>
        /// The specified environment names are compared case insensitively to the current value of
        /// <see cref="IHostingEnvironment.EnvironmentName"/>.
        /// </remarks>        
        public string Include { get; set; }

        /// <summary>
        /// A comma separated list of environment names in which the content will not be rendered.
        /// </summary>
        /// <remarks>
        /// The specified environment names are compared case insensitively to the current value of
        /// <see cref="IHostingEnvironment.EnvironmentName"/>.
        /// </remarks>
        public string Exclude { get; set; }

        public DependencyType Type { get; set; }
                
        public ScriptLocation Location { get; set; }
                
        public int Priority { get; set; }
                
        public string Path { get; set; }        

        public override int Order
        {
            get
            {
                return -1000;
            }
        }

        protected IHostingEnvironment HostingEnvironment { get; }

        public RequireTagHelper(IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
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
                var dependencyLoader = DependencyManager.GetLoader(_httpContextAccessor.HttpContext);
                
                if (!dependencyLoader.DependencyFiles.Any(d => d.FilePath.ToLower() == Path.ToLower()))
                {
                    if (HasEnvironment())
                    {
                        dependencyLoader.DependencyFiles.Add(new DependencyFile
                        {
                            DependencyType = Type,
                            ScriptLocation = Location != null ? Location : ScriptLocation.BodyEnd,
                            FilePath = Path,
                            Priority = Priority > 0 ? Priority : Priority + 100,
                            Attributes = output.Attributes.ToDictionary(k => k.Name, v => v.Value)
                        });
                    }
                }
                //output.Content.SetContent(null);
                output.SuppressOutput();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool HasEnvironment()
        {
            var currentEnvironmentName = HostingEnvironment.EnvironmentName?.Trim();
            bool hasEnvironment = true;

            if (string.IsNullOrEmpty(Include) && string.IsNullOrEmpty(Exclude))
                return hasEnvironment; //by default add the dependency


            if (!string.IsNullOrEmpty(Include))
            {
                var includeTokenizer = new StringTokenizer(Include, NameSeparator);
                hasEnvironment = includeTokenizer.Any(t => t.HasValue && t.Length > 0 && t.Equals(currentEnvironmentName, StringComparison.OrdinalIgnoreCase));

            }

            if (!string.IsNullOrEmpty(Exclude))
            {
                var excludeTokenizer = new StringTokenizer(Exclude, NameSeparator);
                hasEnvironment = excludeTokenizer.Any(t => t.HasValue && t.Length > 0 && !t.Equals(currentEnvironmentName, StringComparison.OrdinalIgnoreCase)); 
            }

            return hasEnvironment;

        }


    }
}
