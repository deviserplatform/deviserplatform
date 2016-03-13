using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("div", Attributes = TitleAttributeName)]
    public class TestTagHelper : TagHelper
    {
        private const string TitleAttributeName = "sde-title";
        private IHtmlHelper htmlHelper;

        public TestTagHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        [HtmlAttributeName(TitleAttributeName)]
        public string Title { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {   
            output.PostContent.Append("This is from Taghelper" + Title);
        }

    }
}
