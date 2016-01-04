using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Mvc.Logging;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Mvc.Controllers;
using Microsoft.AspNet.Mvc.Abstractions;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleActionInvoker : ControllerActionInvoker, IModuleActionInvoker
    {
        private static readonly IFilterMetadata[] EmptyFilterArray = new IFilterMetadata[0];
        private readonly IReadOnlyList<IFilterProvider> _filterProviders;
        private readonly IControllerActionArgumentBinder _argumentBinder;

        protected new object Instance { get; private set; }

        public ModuleActionInvoker(
            ActionContext actionContext,
            IReadOnlyList<IFilterProvider> filterProviders,
            IControllerFactory controllerFactory,
            ControllerActionDescriptor descriptor,
            IReadOnlyList<IInputFormatter> inputFormatters,
            IReadOnlyList<IOutputFormatter> outputFormatters,
            IControllerActionArgumentBinder controllerActionArgumentBinder,
            IReadOnlyList<IModelBinder> modelBinders,
            IReadOnlyList<IModelValidatorProvider> modelValidatorProviders,
            IReadOnlyList<IValueProviderFactory> valueProviderFactories,
            IActionBindingContextAccessor actionBindingContextAccessor,
            ILogger logger,
            DiagnosticSource diagnosticSource,
            int maxModelValidationErrors)
            : base(
                  actionContext,
                  filterProviders,
                  controllerFactory,
                  descriptor,
                  inputFormatters,
                  outputFormatters,
                  controllerActionArgumentBinder,
                  modelBinders,
                  modelValidatorProviders,
                  valueProviderFactories,
                  actionBindingContextAccessor,
                  logger,
                  diagnosticSource,
                  maxModelValidationErrors)
        {
            _filterProviders = filterProviders;
            _argumentBinder = controllerActionArgumentBinder;
        }

        public async Task<IActionResult> InvokeAction()
        {
            //_cursor.Reset();
            var _filters = GetFilters();

            Instance = CreateInstance();

            var arguments = await BindActionArgumentsAsync(ActionContext, ActionBindingContext);

            var _actionExecutingContext = new ActionExecutingContext(
                ActionContext,
                _filters,
                arguments,
                Instance);
                        
            var result = await InvokeActionAsync(_actionExecutingContext);

            return result;
        }

        protected override Task<IDictionary<string, object>> BindActionArgumentsAsync(
           ActionContext context,
           ActionBindingContext bindingContext)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            return _argumentBinder.BindActionArgumentsAsync(context, bindingContext, Instance);
        }


        private IFilterMetadata[] GetFilters()
        {
            var filterDescriptors = ActionContext.ActionDescriptor.FilterDescriptors;
            var items = new List<FilterItem>(filterDescriptors.Count);
            for (var i = 0; i < filterDescriptors.Count; i++)
            {
                items.Add(new FilterItem(filterDescriptors[i]));
            }

            var context = new FilterProviderContext(ActionContext, items);
            for (var i = 0; i < _filterProviders.Count; i++)
            {
                _filterProviders[i].OnProvidersExecuting(context);
            }

            for (var i = _filterProviders.Count - 1; i >= 0; i--)
            {
                _filterProviders[i].OnProvidersExecuted(context);
            }

            var count = 0;
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Filter != null)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                return EmptyFilterArray;
            }
            else
            {
                var filters = new IFilterMetadata[count];
                for (int i = 0, j = 0; i < items.Count; i++)
                {
                    var filter = items[i].Filter;
                    if (filter != null)
                    {
                        filters[j++] = filter;
                    }
                }

                return filters;
            }
        }

        
    }


}
