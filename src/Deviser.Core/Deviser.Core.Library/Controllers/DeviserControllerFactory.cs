using Deviser.Admin;
using Deviser.Admin.Web.Controllers;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Internal;
using Deviser.Core.Common.Module;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ContentResult = Deviser.Core.Common.DomainTypes.ContentResult;
using IActionInvoker = Deviser.Core.Library.Internal.IActionInvoker;
using Module = Deviser.Core.Common.DomainTypes.Module;

namespace Deviser.Core.Library.Controllers
{
    public class DeviserControllerFactory : IDeviserControllerFactory
    {
        private static readonly IReadOnlyList<ActionDescriptor> EmptyActions = Array.Empty<ActionDescriptor>();

        private readonly ILogger<DeviserControllerFactory> _logger;
        private readonly IActionInvoker _actionInvoker;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        //private readonly ActionConstraintCache _actionConstraintCache;
        private readonly IActionSelector _actionSelector;
        private readonly IScopeService _scopeService;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleRegistry _moduleRegistry;
        private Cache _cache;

        public DeviserControllerFactory(ILogger<DeviserControllerFactory> logger,
            IActionInvoker actionInvoker,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IActionSelector actionSelector,
            IScopeService scopeService,
            IHtmlHelper htmlHelper,
            IModuleRepository moduleRepository,
            IModuleRegistry moduleRegistry)
        {
            _logger = logger;
            _actionInvoker = actionInvoker;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            //_actionConstraintCache = container.Resolve<ActionConstraintCache>();
            _actionSelector = actionSelector;
            _htmlHelper = htmlHelper;
            _moduleRepository = moduleRepository;
            _moduleRegistry = moduleRegistry;
            _scopeService = scopeService;
        }

        /// <summary>
        /// This method gets current page "View" actions of all modules
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<ContentResult>>> GetPageModuleResults(ActionContext actionContext)
        {
            var actionResults = new Dictionary<string, List<ContentResult>>();
            var currentPage = _scopeService.PageContext.CurrentPage;
            if (currentPage.PageModule == null || currentPage.PageModule.Count <= 0) return actionResults;
            currentPage.PageModule = currentPage.PageModule.Where(pm => pm.IsActive).ToList();
            foreach (var pageModule in currentPage.PageModule)
            {
                var module = _moduleRepository.GetModule(pageModule.ModuleId);
                var moduleView = module.ModuleView.FirstOrDefault(ma => ma.Id == pageModule.ModuleViewId);
                var moduleContext = new ModuleContext(module, pageModule);
                if (moduleView == null) continue;

                List<ContentResult> contentResults;
                var containerId = pageModule.ContainerId.ToString();
                //Prepare the result object
                if (actionResults.ContainsKey(containerId))
                {
                    contentResults = actionResults[containerId];
                }
                else
                {
                    contentResults = new List<ContentResult>();
                    actionResults.Add(containerId, contentResults);
                }

                try
                {
                    var actionResult = await ExecuteModuleController(actionContext, moduleContext, moduleView);
                    var moduleResult = GetModuleResult(actionResult, moduleContext);
                    contentResults.Add(new ContentResult
                    {
                        HtmlResult = moduleResult,
                        SortOrder = pageModule.SortOrder
                    });
                }
                catch (Exception ex)
                {
                    var actionResult = "Module load exception has been occured";
                    contentResults.Add(new ContentResult
                    {
                        //Result = actionResult,
                        HtmlResult = new HtmlString(actionResult),
                        SortOrder = pageModule.SortOrder
                    });
                    _logger.LogError("Module load exception has been occured", ex);
                }
            }

            return actionResults;
        }

        //public async Task<string> GetModuleEditResultAsString(ActionContext actionContext, PageModule pageModule, Guid moduleEditActionId)
        //{
        //    string actionResult = string.Empty;
        //    try
        //    {
        //        var module = _moduleRepository.Get(pageModule.ModuleId);
        //        ModuleView moduleView = module.ModuleView.FirstOrDefault(ma => ma.Id == moduleEditActionId); //It refers PageModule's Edit ModuleViewType
        //        ModuleContext moduleContext = new ModuleContext();
        //        moduleContext.ModuleInfo = module; //Context should be PageModule's instance, but not Edit ModuleViewType
        //        moduleContext.PageModuleId = pageModule.Id;
        //        if (module != null && moduleView != null)
        //        {
        //            actionResult = await ExecuteModuleControllerAsString(actionContext, moduleContext, moduleView);
        //            actionResult = $"<div class=\"dev-module-container\" data-module=\"{moduleContext.ModuleInfo.Name}\" data-page-module-id=\"{moduleContext.PageModuleId}\">{actionResult}</div>";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        actionResult = "Module load exception has been occured";
        //        _logger.LogError("Module load exception has been occured", ex);
        //    }

        //    return actionResult;
        //}

        public async Task<IHtmlContent> GetModuleEditResult(ActionContext actionContext, PageModule pageModule, Guid moduleEditActionId)
        {
            IHtmlContent editResult = new HtmlString(string.Empty);
            try
            {
                var module = _moduleRepository.GetModule(pageModule.ModuleId);
                var moduleView = module.ModuleView.FirstOrDefault(ma => ma.Id == moduleEditActionId); //It refers PageModule's Edit ModuleViewType
                var moduleContext = new ModuleContext(module, pageModule);//Context should be PageModule's instance, but not Edit ModuleViewType
                if (moduleView != null)
                {
                    var actionResult = await ExecuteModuleController(actionContext, moduleContext, moduleView);
                    editResult = GetModuleResult(actionResult, moduleContext);
                }
            }
            catch (Exception ex)
            {
                editResult = new HtmlString("Module load exception has been occured");
                _logger.LogError("Module load exception has been occured", ex);
            }

            return editResult;
        }

        public async Task<IHtmlContent> GetAdminPageResult(ActionContext actionContext)
        {
            var currentPage = _scopeService.PageContext.CurrentPage;
            if (currentPage.AdminPage == null)
            {
                return null;
            }

            var moduleName = currentPage.AdminPage.ModuleName;
            var modelName = currentPage.AdminPage.ModelName;
            var moduleMetaInfo = _moduleRegistry.GetModuleMetaInfoByModuleName(moduleName);

            var assembly = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.EntryPointAssembly).FirstOrDefault(a => a.FullName == moduleMetaInfo.ModuleAssemblyFullName);
            var controllerTypeInfo = assembly.GetDerivedTypeInfos(typeof(IAdminController)).FirstOrDefault();
            var controllerName = controllerTypeInfo.Name.Replace("Controller", "");
            var actionName = nameof(AdminController<IAdminConfigurator>.Admin);


            var routeContext = new RouteContext(actionContext.HttpContext);
            routeContext.RouteData = new RouteData();
            //routeContext.RouteData.Values.Add("area", moduleName);
            //routeContext.RouteData.Values.Add("controller", controllerName);
            //routeContext.RouteData.Values.Add("action", actionName);
            //routeContext.RouteData.Values.Add("entity", entityName);

            var routeParams = new RouteValueDictionary(new { model = modelName });
            var routeValues = new RouteValueDictionary(new { area = moduleName, controller = controllerName, action = actionName });

            foreach (var router in actionContext.RouteData.Routers)
            {
                routeContext.RouteData.PushState(router, null, null);
            }

            routeContext.RouteData.PushState(null, routeValues, null);
            routeContext.RouteData.PushState(null, routeParams, null);

            var adminActionContext = GetActionContext(routeContext);


            //var invoker = _moduleInvokerProvider.CreateInvoker(moduleActionContext);
            //var result = await invoker.InvokeAction() as ViewResult;



            var result = await _actionInvoker.InvokeAction(actionContext.HttpContext, controllerTypeInfo.Namespace, controllerName, actionName, adminActionContext) as ViewResult;

            var htmlResult = result.ExecuteResultToHTML(adminActionContext);
            return htmlResult;
        }

        private IHtmlContent GetModuleResult(IHtmlContent actionResult, ModuleContext moduleContext)
        {
            var moduleContainer = new TagBuilder("div");
            moduleContainer.Attributes.Add("class", "dev-module-container");
            moduleContainer.Attributes.Add("data-module", moduleContext.ModuleInfo.Name);
            moduleContainer.Attributes.Add("data-page-module-id", moduleContext.PageModuleId.ToString());
            moduleContainer.InnerHtml.AppendHtml(actionResult);
            return moduleContainer;
        }

        //private async Task<string> ExecuteModuleControllerAsString(ActionContext actionContext, ModuleContext moduleContext, ModuleView moduleView)
        //{
        //    if (actionContext == null)
        //    {
        //        return null;
        //    }

        //    ActionContext moduleActionContext = GetModuleActionContext(actionContext, moduleContext, moduleView);

        //    //var invoker = _moduleInvokerProvider.CreateInvoker(moduleActionContext);
        //    //var result = await invoker.InvokeAction() as ViewResult;
        //    var result = await _actionInvoker.InvokeAction(actionContext.HttpContext, moduleView, moduleActionContext) as ViewResult;

        //    string strResult = result.ExecuteResultToString(moduleActionContext);
        //    return strResult;
        //}

        private async Task<IHtmlContent> ExecuteModuleController(ActionContext actionContext, ModuleContext moduleContext, ModuleView moduleView)
        {
            if (actionContext == null)
            {
                return null;
            }

            var moduleActionContext = GetModuleActionContext(actionContext, moduleContext, moduleView);

            //var invoker = _moduleInvokerProvider.CreateInvoker(moduleActionContext);
            //var result = await invoker.InvokeAction() as ViewResult;
            var result = await _actionInvoker.InvokeAction(actionContext.HttpContext, moduleView, moduleActionContext, moduleContext) as ViewResult;

            var htmlResult = result.ExecuteResultToHTML(moduleActionContext);
            return htmlResult;
        }

        private ActionContext GetModuleActionContext(ActionContext actionContext, ModuleContext moduleContext, ModuleView moduleView)
        {
            var routeContext = new RouteContext(actionContext.HttpContext);
            routeContext.RouteData = new RouteData();
            routeContext.RouteData.Values.Add("area", moduleContext.ModuleInfo.Name);
            routeContext.RouteData.Values.Add("pageModuleId", moduleContext.PageModuleId);
            routeContext.RouteData.Values.Add("controller", moduleView.ControllerName);
            routeContext.RouteData.Values.Add("action", moduleView.ActionName);
            routeContext.RouteData.PushState(actionContext.RouteData.Routers.FirstOrDefault(), null, null);

            return GetActionContext(routeContext, true);
        }

        private ActionContext GetActionContext(RouteContext routeContext, bool isMVCAction = false)
        {

            IReadOnlyList<ActionDescriptor> actionDescriptors = null;
            if (isMVCAction)
            {
                //MVC Route Actions
                actionDescriptors = _actionSelector.SelectCandidates(routeContext);
            }
            else
            {
                var actions = _actionDescriptorCollectionProvider.ActionDescriptors;

                //Attributed Actions
                var attributeRoutedActions = actions.Items.Where(a => a.AttributeRouteInfo?.Template != null && !a.AttributeRouteInfo.SuppressPathMatching).ToList();

                actionDescriptors = attributeRoutedActions.Where(ad => 
                    ad.RouteValues.ContainsKey("area") && 
                    string.Equals(ad.RouteValues["area"], routeContext.RouteData.Values["area"] as string, StringComparison.OrdinalIgnoreCase) && 
                    string.Equals(ad.RouteValues["controller"], routeContext.RouteData.Values["controller"] as string, StringComparison.OrdinalIgnoreCase) && 
                    string.Equals(ad.RouteValues["action"], routeContext.RouteData.Values["action"] as string, StringComparison.OrdinalIgnoreCase)).ToList();
            }



            var actionDescriptor = _actionSelector.SelectBestCandidate(routeContext, actionDescriptors); //SelectBestCandidate(routeContext, actionDescriptors);
            if (actionDescriptor == null)
                throw new NullReferenceException("Action cannot be located, please check whether module has been installed properly");

            var moduleActionContext = new ActionContext(routeContext.HttpContext, routeContext.RouteData, actionDescriptor);
            return moduleActionContext;
        }

        private Cache Current
        {
            get
            {
                var actions = _actionDescriptorCollectionProvider.ActionDescriptors;
                var cache = Volatile.Read(ref _cache);

                if (cache != null && cache.Version == actions.Version)
                {
                    return cache;
                }

                cache = new Cache(actions);
                Volatile.Write(ref _cache, cache);
                return cache;
            }
        }

        public IReadOnlyList<ActionDescriptor> SelectCandidates(RouteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var cache = Current;

            // The Cache works based on a string[] of the route values in a pre-calculated order. This code extracts
            // those values in the correct order.
            var keys = cache.RouteKeys;
            var values = new string[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                context.RouteData.Values.TryGetValue(keys[i], out var value);

                if (value != null)
                {
                    values[i] = value as string ?? Convert.ToString(value);
                }
            }

            if (cache.OrdinalEntries.TryGetValue(values, out var matchingRouteValues) ||
                cache.OrdinalIgnoreCaseEntries.TryGetValue(values, out matchingRouteValues))
            {
                return matchingRouteValues;
            }

            return EmptyActions;
        }

        //public ActionDescriptor SelectBestCandidate(RouteContext context, IReadOnlyList<ActionDescriptor> candidates)
        //{
        //    if (context == null)
        //    {
        //        throw new ArgumentNullException(nameof(context));
        //    }

        //    if (candidates == null)
        //    {
        //        throw new ArgumentNullException(nameof(candidates));
        //    }

        //    var matches = EvaluateActionConstraints(context, candidates);

        //    var finalMatches = SelectBestActions(matches);
        //    if (finalMatches == null || finalMatches.Count == 0)
        //    {
        //        return null;
        //    }
        //    else if (finalMatches.Count == 1)
        //    {
        //        var selectedAction = finalMatches[0];

        //        return selectedAction;
        //    }
        //    else
        //    {
        //        var actionNames = string.Join(
        //            Environment.NewLine,
        //            finalMatches.Select(a => a.DisplayName));

        //        _logger.AmbiguousActions(actionNames);

        //        var message = Resources.FormatDefaultActionSelector_AmbiguousActions(
        //            Environment.NewLine,
        //            actionNames);

        //        throw new AmbiguousActionException(message);
        //    }
        //}

        /// <summary>
        /// Returns the set of best matching actions.
        /// </summary>
        /// <param name="actions">The set of actions that satisfy all constraints.</param>
        /// <returns>A list of the best matching actions.</returns>
        protected virtual IReadOnlyList<ActionDescriptor> SelectBestActions(IReadOnlyList<ActionDescriptor> actions)
        {
            return actions;
        }

        //private IReadOnlyList<ActionDescriptor> EvaluateActionConstraints(
        //    RouteContext context,
        //    IReadOnlyList<ActionDescriptor> actions)
        //{
        //    var candidates = new List<ActionSelectorCandidate>();

        //    // Perf: Avoid allocations
        //    for (var i = 0; i < actions.Count; i++)
        //    {
        //        var action = actions[i];
        //        var constraints = _actionConstraintCache.GetActionConstraints(context.HttpContext, action);
        //        candidates.Add(new ActionSelectorCandidate(action, constraints));
        //    }

        //    var matches = EvaluateActionConstraintsCore(context, candidates, startingOrder: null);

        //    List<ActionDescriptor> results = null;
        //    if (matches != null)
        //    {
        //        results = new List<ActionDescriptor>(matches.Count);
        //        // Perf: Avoid allocations
        //        for (var i = 0; i < matches.Count; i++)
        //        {
        //            var candidate = matches[i];
        //            results.Add(candidate.Action);
        //        }
        //    }

        //    return results;
        //}

        private IReadOnlyList<ActionSelectorCandidate> EvaluateActionConstraintsCore(
            RouteContext context,
            IReadOnlyList<ActionSelectorCandidate> candidates,
            int? startingOrder)
        {
            // Find the next group of constraints to process. This will be the lowest value of
            // order that is higher than startingOrder.
            int? order = null;

            // Perf: Avoid allocations
            foreach (var candidate in candidates)
            {
                if (candidate.Constraints == null) continue;
                foreach (var constraint in candidate.Constraints)
                {
                    if ((startingOrder == null || constraint.Order > startingOrder) &&
                        (order == null || constraint.Order < order))
                    {
                        order = constraint.Order;
                    }
                }
            }

            // If we don't find a next then there's nothing left to do.
            if (order == null)
            {
                return candidates;
            }

            // Since we have a constraint to process, bisect the set of actions into those with and without a
            // constraint for the current order.
            var actionsWithConstraint = new List<ActionSelectorCandidate>();
            var actionsWithoutConstraint = new List<ActionSelectorCandidate>();

            var constraintContext = new ActionConstraintContext();
            constraintContext.Candidates = candidates;
            constraintContext.RouteContext = context;

            // Perf: Avoid allocations
            foreach (var candidate in candidates)
            {
                var isMatch = true;
                var foundMatchingConstraint = false;

                if (candidate.Constraints != null)
                {
                    constraintContext.CurrentCandidate = candidate;
                    foreach (var constraint in candidate.Constraints)
                    {
                        if (constraint.Order != order) continue;
                        foundMatchingConstraint = true;

                        if (constraint.Accept(constraintContext)) continue;
                        isMatch = false;
                        _logger.ConstraintMismatch(
                            candidate.Action.DisplayName,
                            candidate.Action.Id,
                            constraint);
                        break;
                    }
                }

                if (isMatch && foundMatchingConstraint)
                {
                    actionsWithConstraint.Add(candidate);
                }
                else if (isMatch)
                {
                    actionsWithoutConstraint.Add(candidate);
                }
            }

            // If we have matches with constraints, those are better so try to keep processing those
            if (actionsWithConstraint.Count <= 0)
                return actionsWithoutConstraint.Count == 0
                    ? null
                    : EvaluateActionConstraintsCore(context, actionsWithoutConstraint, order);

            var matches = EvaluateActionConstraintsCore(context, actionsWithConstraint, order);
            if (matches?.Count > 0)
            {
                return matches;
            }

            // If the set of matches with constraints can't work, then process the set without constraints.
            return actionsWithoutConstraint.Count == 0 ? null : EvaluateActionConstraintsCore(context, actionsWithoutConstraint, order);
        }


        private class Cache
        {
            public Cache(ActionDescriptorCollection actions)
            {
                // We need to store the version so the cache can be invalidated if the actions change.
                Version = actions.Version;

                // We need to build two maps for all of the route values.
                OrdinalEntries = new Dictionary<string[], List<ActionDescriptor>>(StringArrayComparer.Ordinal);
                OrdinalIgnoreCaseEntries = new Dictionary<string[], List<ActionDescriptor>>(StringArrayComparer.OrdinalIgnoreCase);

                // We need to first identify of the keys that action selection will look at (in route data). 
                // We want to only consider conventionally routed actions here.
                var routeKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var action in actions.Items)
                {
                    if (action.AttributeRouteInfo != null) continue;

                    // This is a conventionally routed action - so make sure we include its keys in the set of
                    // known route value keys.
                    foreach (var kvp in action.RouteValues)
                    {
                        routeKeys.Add(kvp.Key);
                    }
                }

                // We need to hold on to an ordered set of keys for the route values. We'll use these later to
                // extract the set of route values from an incoming request to compare against our maps of known
                // route values.
                RouteKeys = routeKeys.ToArray();

                foreach (var action in actions.Items)
                {
                    if (action.AttributeRouteInfo != null)
                    {
                        // This only handles conventional routing. Ignore attribute routed actions.
                        continue;
                    }

                    // This is a conventionally routed action - so we need to extract the route values associated
                    // with this action (in order) so we can store them in our dictionaries.
                    var routeValues = new string[RouteKeys.Length];
                    for (var j = 0; j < RouteKeys.Length; j++)
                    {
                        action.RouteValues.TryGetValue(RouteKeys[j], out routeValues[j]);
                    }

                    if (!OrdinalIgnoreCaseEntries.TryGetValue(routeValues, out var entries))
                    {
                        entries = new List<ActionDescriptor>();
                        OrdinalIgnoreCaseEntries.Add(routeValues, entries);
                    }

                    entries.Add(action);

                    // We also want to add the same (as in reference equality) list of actions to the ordinal entries.
                    // We'll keep updating `entries` to include all of the actions in the same equivalence class -
                    // meaning, all conventionally routed actions for which the route values are equalignoring case.
                    //
                    // `entries` will appear in `OrdinalIgnoreCaseEntries` exactly once and in `OrdinalEntries` once
                    // for each variation of casing that we've seen.
                    if (!OrdinalEntries.ContainsKey(routeValues))
                    {
                        OrdinalEntries.Add(routeValues, entries);
                    }
                }
            }

            public int Version { get; }

            public string[] RouteKeys { get; }

            public Dictionary<string[], List<ActionDescriptor>> OrdinalEntries { get; }

            public Dictionary<string[], List<ActionDescriptor>> OrdinalIgnoreCaseEntries { get; }
        }

        private class StringArrayComparer : IEqualityComparer<string[]>
        {
            public static readonly StringArrayComparer Ordinal = new StringArrayComparer(StringComparer.Ordinal);

            public static readonly StringArrayComparer OrdinalIgnoreCase = new StringArrayComparer(StringComparer.OrdinalIgnoreCase);

            private readonly StringComparer _valueComparer;

            private StringArrayComparer(StringComparer valueComparer)
            {
                _valueComparer = valueComparer;
            }

            public bool Equals(string[] x, string[] y)
            {
                if (object.ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x == null ^ y == null)
                {
                    return false;
                }

                if (x.Length != y.Length)
                {
                    return false;
                }

                for (var i = 0; i < x.Length; i++)
                {
                    if (string.IsNullOrEmpty(x[i]) && string.IsNullOrEmpty(y[i]))
                    {
                        continue;
                    }

                    if (!_valueComparer.Equals(x[i], y[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(string[] obj)
            {
                if (obj == null)
                {
                    return 0;
                }

                var hash = new HashCodeCombiner();
                foreach (var t in obj)
                {
                    var o = t;

                    // Route values define null and "" to be equivalent.
                    if (string.IsNullOrEmpty(o))
                    {
                        o = null;
                    }
                    hash.Add(o, _valueComparer);
                }

                return hash.CombinedHash;
            }
        }

        internal struct HashCodeCombiner
        {
            private long _combinedHash64;

            public int CombinedHash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _combinedHash64.GetHashCode();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private HashCodeCombiner(long seed)
            {
                _combinedHash64 = seed;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(IEnumerable e)
            {
                if (e == null)
                {
                    Add(0);
                }
                else
                {
                    var count = 0;
                    foreach (var o in e)
                    {
                        Add(o);
                        count++;
                    }
                    Add(count);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator int(HashCodeCombiner self)
            {
                return self.CombinedHash;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(int i)
            {
                _combinedHash64 = ((_combinedHash64 << 5) + _combinedHash64) ^ i;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(string s)
            {
                var hashCode = s?.GetHashCode() ?? 0;
                Add(hashCode);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(object o)
            {
                var hashCode = o?.GetHashCode() ?? 0;
                Add(hashCode);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add<TValue>(TValue value, IEqualityComparer<TValue> comparer)
            {
                var hashCode = value != null ? comparer.GetHashCode(value) : 0;
                Add(hashCode);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static HashCodeCombiner Start()
            {
                return new HashCodeCombiner(0x1505L);
            }
        }

        public void Dispose()
        {
            _actionInvoker.Dispose();
        }
    }


}
