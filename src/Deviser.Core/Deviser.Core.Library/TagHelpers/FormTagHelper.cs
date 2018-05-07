using System;
using System.Collections.Generic;
using System.ComponentModel;
using Deviser.Core.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Deviser.Core.Library.TagHelpers
{
    // <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; elements.
    /// </summary>
    [HtmlTargetElement("form", Attributes = ActionAttributeName)]
    [HtmlTargetElement("form", Attributes = AntiforgeryAttributeName)]
    [HtmlTargetElement("form", Attributes = ModuleAttributeName)]
    [HtmlTargetElement("form", Attributes = ControllerAttributeName)]
    //[HtmlTargetElement("form", Attributes = RouteAttributeName)]
    [HtmlTargetElement("form", Attributes = RouteValuesDictionaryName)]
    [HtmlTargetElement("form", Attributes = RouteValuesPrefix + "*")]
    public class FormTagHelper : TagHelper
    {
        private const string ActionAttributeName = "dev-action";
        private const string AntiforgeryAttributeName = "dev-antiforgery";
        private const string ModuleAttributeName = "dev-module";
        private const string ControllerAttributeName = "dev-controller";
        //private const string RouteAttributeName = "dev-route";
        private const string RouteValuesDictionaryName = "dev-all-route-data";
        private const string RouteValuesPrefix = "dev-route-";
        private const string HtmlActionAttributeName = "action";
        private IDictionary<string, string> _routeValues;

        private const string route = "moduleRoute";

        /// <summary>
        /// Creates a new <see cref="FormTagHelper"/>.
        /// </summary>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public FormTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        /// <inheritdoc />
        public override int Order
        {
            get
            {
                return -1000;
            }
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        /// <summary>
        /// The name of the action method.
        /// </summary>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// The name of the controller.
        /// </summary>
        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        /// <summary>
        /// The name of the area.
        /// </summary>
        [HtmlAttributeName(ModuleAttributeName)]
        public string Area { get; set; }

        /// <summary>
        /// Whether the antiforgery token should be generated.
        /// </summary>
        /// <value>Defaults to <c>false</c> if user provides an <c>action</c> attribute
        /// or if the <c>method</c> is <see cref="FormMethod.Get"/>; <c>true</c> otherwise.</value>
        [HtmlAttributeName(AntiforgeryAttributeName)]
        public bool? Antiforgery { get; set; }

        /// <summary>
        /// Name of the route.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Action"/> or <see cref="Controller"/> is non-<c>null</c>.
        /// </remarks>
        //[HtmlAttributeName(RouteAttributeName)]
        //public string Route { get; set; }

        /// <summary>
        /// The HTTP method to use.
        /// </summary>
        /// <remarks>Passed through to the generated HTML in all cases.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Method { get; set; }

        /// <summary>
        /// Additional parameters for the route.
        /// </summary>
        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Does nothing if user provides an <c>action</c> attribute and <see cref="Antiforgery"/> is <c>null</c> or
        /// <c>false</c>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <c>action</c> attribute is provided and <see cref="Action"/> or <see cref="Controller"/> are
        /// non-<c>null</c> or if the user provided <c>dev-route-*</c> attributes.
        /// </exception>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (Area == null)
            {
                throw new ArgumentNullException(nameof(Area));
            }

            if (Method != null)
            {
                output.CopyHtmlAttribute(nameof(Method), context);
            }

            var antiforgeryDefault = true;

            // If "action" is already set, it means the user is attempting to use a normal <form>.
            if (output.Attributes.ContainsName(HtmlActionAttributeName))
            {
                if (Action != null || Controller != null || Area != null || RouteValues.Count != 0)
                {
                    // User also specified bound attributes we cannot use.
                    throw new InvalidOperationException(string.Format("Cannot Override action {0}, {1}, {2}, {3}, {4}, {5}",
                            "<form>",
                            HtmlActionAttributeName,
                            ActionAttributeName,
                            ControllerAttributeName,
                            ModuleAttributeName,
                            RouteValuesPrefix));
                }

                // User is using the FormTagHelper like a normal <form> tag. Antiforgery default should be false to
                // not force the antiforgery token on the user.
                antiforgeryDefault = false;
            }
            else
            {
                IDictionary<string, object> routeValues = null;
                if (_routeValues != null && _routeValues.Count > 0)
                {
                    // Convert from Dictionary<string, string> to Dictionary<string, object>.
                    routeValues = new Dictionary<string, object>(_routeValues.Count, StringComparer.OrdinalIgnoreCase);
                    foreach (var routeValue in _routeValues)
                    {
                        routeValues.Add(routeValue.Key, routeValue.Value);
                    }
                }

                if (Area != null)
                {
                    if (routeValues == null)
                    {
                        routeValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    }

                    // Unconditionally replace any value from dev-route-area. 
                    routeValues["area"] = Area;
                }

                TagBuilder tagBuilder;
                //if (Route == null)
                //{
                //    tagBuilder = Generator.GenerateForm(
                //        ViewContext,
                //        Action,
                //        Controller,
                //        routeValues,
                //        method: null,
                //        htmlAttributes: null);
                //}
                //if (Action != null || Controller != null)
                //{
                //    // Route and Action or Controller were specified. Can't determine the action attribute.
                //    throw new InvalidOperationException(
                //        Resources.FormatFormTagHelper_CannotDetermineActionWithRouteAndActionOrControllerSpecified(
                //            "<form>",
                //            //RouteAttributeName,
                //            "",
                //            ActionAttributeName,
                //            ControllerAttributeName,
                //            HtmlActionAttributeName));
                //}
                //else
                //{
                //}
                if (Action != null)
                {
                    routeValues.Add("Action", Action);
                }

                if (Controller != null)
                {
                    routeValues.Add("Controller", Controller);
                }

                tagBuilder = Generator.GenerateRouteForm(
                ViewContext,
                Globals.moduleRoute,
                routeValues,
                method: null,
                htmlAttributes: null);


                if (tagBuilder != null)
                {
                    output.MergeAttributes(tagBuilder);
                    output.PostContent.AppendHtml(tagBuilder.InnerHtml);
                }

                if (string.Equals(Method, FormMethod.Get.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    antiforgeryDefault = false;
                }
            }

            if (Antiforgery ?? antiforgeryDefault)
            {
                var antiforgeryTag = Generator.GenerateAntiforgery(ViewContext);
                if (antiforgeryTag != null)
                {
                    output.PostContent.AppendHtml(antiforgeryTag);
                }
            }
        }
    }
}
