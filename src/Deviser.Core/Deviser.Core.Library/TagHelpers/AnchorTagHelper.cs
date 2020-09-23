using Deviser.Core.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("a", Attributes = ActionAttributeName)]
    [HtmlTargetElement("a", Attributes = ControllerAttributeName)]
    [HtmlTargetElement("a", Attributes = ModuleAttributeName)]
    [HtmlTargetElement("a", Attributes = FragmentAttributeName)]
    [HtmlTargetElement("a", Attributes = HostAttributeName)]
    [HtmlTargetElement("a", Attributes = ProtocolAttributeName)]
    [HtmlTargetElement("a", Attributes = RouteAttributeName)]
    [HtmlTargetElement("a", Attributes = RouteValuesDictionaryName)]
    [HtmlTargetElement("a", Attributes = RouteValuesPrefix + "*")]
    public class AnchorTagHelper : TagHelper
    {
        private const string ActionAttributeName = "dev-action";
        private const string ControllerAttributeName = "dev-controller";
        private const string ModuleAttributeName = "dev-module";
        private const string FragmentAttributeName = "dev-fragment";
        private const string HostAttributeName = "dev-host";
        private const string ProtocolAttributeName = "dev-protocol";
        private const string RouteAttributeName = "dev-route";
        private const string RouteValuesDictionaryName = "dev-all-route-data";
        private const string RouteValuesPrefix = "dev-route-";
        private const string Href = "href";
        private IDictionary<string, string> _routeValues;

        /// <summary>
        /// Creates a new <see cref="AnchorTagHelper"/>.
        /// </summary>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public AnchorTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        /// <inheritdoc />
        public override int Order => -1000;

        protected IHtmlGenerator Generator { get; }

        /// <summary>
        /// The name of the action method.
        /// </summary>
        /// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// The name of the controller.
        /// </summary>
        /// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        /// <summary>
        /// The name of the area.
        /// </summary>
        /// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        [HtmlAttributeName(ModuleAttributeName)]
        public string Area { get; set; }

        /// <summary>
        /// The protocol for the URL, such as &quot;http&quot; or &quot;https&quot;.
        /// </summary>
        [HtmlAttributeName(ProtocolAttributeName)]
        public string Protocol { get; set; }

        /// <summary>
        /// The host name.
        /// </summary>
        [HtmlAttributeName(HostAttributeName)]
        public string Host { get; set; }

        /// <summary>
        /// The URL fragment name.
        /// </summary>
        [HtmlAttributeName(FragmentAttributeName)]
        public string Fragment { get; set; }

        /// <summary>
        /// Name of the route.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Action"/> or <see cref="Controller"/> is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }

        /// <summary>
        /// Additional parameters for the route.
        /// </summary>
        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        {
            get =>
                _routeValues ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            set => _routeValues = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="Rendering.ViewContext"/> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <inheritdoc />
        /// <remarks>Does nothing if user provides an <c>href</c> attribute.</remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <c>href</c> attribute is provided and <see cref="Action"/>, <see cref="Controller"/>,
        /// <see cref="Fragment"/>, <see cref="Host"/>, <see cref="Protocol"/>, or <see cref="Route"/> are
        /// non-<c>null</c> or if the user provided <c>dev-route-*</c> attributes. Also thrown if <see cref="Route"/>
        /// and one or both of <see cref="Action"/> and <see cref="Controller"/> are non-<c>null</c>.
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

            // If "href" is already set, it means the user is attempting to use a normal anchor.
            if (output.Attributes.ContainsName(Href))
            {
                if (Action != null ||
                    Controller != null ||
                    Area != null ||
                    Route != null ||
                    Protocol != null ||
                    Host != null ||
                    Fragment != null ||
                    RouteValues.Count != 0)
                {
                    // User specified an href and one of the bound attributes; can't determine the href attribute.
                    throw new InvalidOperationException(
                        String.Format("Cannot Override Href, {0},{1},{2},{3},{4},{5},{6}, {7}, {8}, {9}",
                            "<a>",
                            ActionAttributeName,
                            ControllerAttributeName,
                            ModuleAttributeName,
                            RouteAttributeName,
                            ProtocolAttributeName,
                            HostAttributeName,
                            FragmentAttributeName,
                            RouteValuesPrefix,
                            Href));
                }
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
                    routeValues ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                    // Unconditionally replace any value from dev-route-area. 
                    routeValues["area"] = Area;
                }

                TagBuilder tagBuilder;

               
                //if (Route == null)
                //{
                //    tagBuilder = Generator.GenerateActionLink(
                //        ViewContext,
                //        linkText: string.Empty,
                //        actionName: Action,
                //        controllerName: Controller,
                //        protocol: Protocol,
                //        hostname: Host,
                //        fragment: Fragment,
                //        routeValues: routeValues,
                //        htmlAttributes: null);
                //}
                //else if (Action != null || Controller != null)
                //{
                //    // Route and Action or Controller were specified. Can't determine the href attribute.
                //    throw new InvalidOperationException(
                //        String.Format("Cannot determine href route action or controller specified, {0}, {1}, {2}, {3}, {4}",
                //            "<a>",
                //            RouteAttributeName,
                //            ActionAttributeName,
                //            ControllerAttributeName,
                //            Href));
                //}
                //else
                //{

                //}

                if (Action != null)
                {
                    routeValues.Add("Action", Action);
                }

                if(Controller != null)
                {
                    routeValues.Add("Controller", Controller);
                }

                tagBuilder = Generator.GenerateRouteLink(
                        ViewContext,
                        linkText: string.Empty,
                        routeName: Globals.moduleRoute,
                        protocol: Protocol,
                        hostName: Host,
                        fragment: Fragment,
                        routeValues: routeValues,
                        htmlAttributes: null);

                if (tagBuilder != null)
                {
                    output.MergeAttributes(tagBuilder);
                }
            }
        }
    }
}
