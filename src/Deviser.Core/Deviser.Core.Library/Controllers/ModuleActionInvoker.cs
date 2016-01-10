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
using Microsoft.AspNet.Mvc.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleActionInvoker : ControllerActionInvoker, IModuleActionInvoker
    {
        private static readonly IFilterMetadata[] EmptyFilterArray = new IFilterMetadata[0];
        private readonly IReadOnlyList<IFilterProvider> _filterProviders;
        private ActionExecutingContext _actionExecutingContext;
        private ActionExecutedContext _actionExecutedContext;
        private readonly IControllerActionArgumentBinder _argumentBinder;

        private IFilterMetadata[] _filters;
        private FilterCursor _cursor;

        DiagnosticSource _diagnosticSource;

        private const string ActionFilterShortCircuitLogMessage =
            "Request was short circuited at action filter '{ActionFilter}'.";

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
            _diagnosticSource = diagnosticSource;
        }

        public async Task<IActionResult> InvokeAction()
        {
            //_cursor.Reset();
            _filters = GetFilters();
            _cursor = new FilterCursor(_filters);

            Instance = CreateInstance();

            var arguments = await BindActionArgumentsAsync(ActionContext, ActionBindingContext);

            _actionExecutingContext = new ActionExecutingContext(
                ActionContext,
                _filters,
                arguments,
                Instance);

            //var result = await InvokeActionAsync(_actionExecutingContext);
            await InvokeActionFilterAsync();

            return _actionExecutedContext.Result;
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

        private async Task<ActionExecutedContext> InvokeActionFilterAsync()
        {
            Debug.Assert(_actionExecutingContext != null);
            if (_actionExecutingContext.Result != null)
            {
                // If we get here, it means that an async filter set a result AND called next(). This is forbidden.
                var message = Resources.FormatAsyncActionFilter_InvalidShortCircuit(
                    typeof(IAsyncActionFilter).Name,
                    nameof(ActionExecutingContext.Result),
                    typeof(ActionExecutingContext).Name,
                    typeof(ActionExecutionDelegate).Name);

                throw new InvalidOperationException(message);
            }

            var item = _cursor.GetNextFilter<IActionFilter, IAsyncActionFilter>();
            try
            {
                if (item.FilterAsync != null)
                {
                    _diagnosticSource.BeforeOnActionExecution(
                        _actionExecutingContext,
                        item.FilterAsync);

                    await item.FilterAsync.OnActionExecutionAsync(_actionExecutingContext, InvokeActionFilterAsync);

                    _diagnosticSource.AfterOnActionExecution(
                        _actionExecutingContext.ActionDescriptor,
                        _actionExecutedContext,
                        item.FilterAsync);

                    if (_actionExecutedContext == null)
                    {
                        // If we get here then the filter didn't call 'next' indicating a short circuit

                        Logger.LogVerbose(ActionFilterShortCircuitLogMessage, item.FilterAsync.GetType().FullName);

                        _actionExecutedContext = new ActionExecutedContext(
                            _actionExecutingContext,
                            _filters,
                            Instance)
                        {
                            Canceled = true,
                            Result = _actionExecutingContext.Result,
                        };
                    }
                }
                else if (item.Filter != null)
                {
                    _diagnosticSource.BeforeOnActionExecuting(
                        _actionExecutingContext,
                        item.Filter);

                    item.Filter.OnActionExecuting(_actionExecutingContext);

                    _diagnosticSource.AfterOnActionExecuting(
                        _actionExecutingContext,
                        item.Filter);

                    if (_actionExecutingContext.Result != null)
                    {
                        // Short-circuited by setting a result.

                        Logger.LogVerbose(ActionFilterShortCircuitLogMessage, item.Filter.GetType().FullName);

                        _actionExecutedContext = new ActionExecutedContext(
                            _actionExecutingContext,
                            _filters,
                            Instance)
                        {
                            Canceled = true,
                            Result = _actionExecutingContext.Result,
                        };
                    }
                    else
                    {
                        _diagnosticSource.BeforeOnActionExecuted(
                            _actionExecutingContext.ActionDescriptor,
                            _actionExecutedContext,
                            item.Filter);

                        item.Filter.OnActionExecuted(await InvokeActionFilterAsync());

                        _diagnosticSource.BeforeOnActionExecuted(
                            _actionExecutingContext.ActionDescriptor,
                            _actionExecutedContext,
                            item.Filter);
                    }
                }
                else
                {
                    // All action filters have run, execute the action method.
                    IActionResult result = null;

                    try
                    {
                        _diagnosticSource.BeforeActionMethod(
                            ActionContext,
                            _actionExecutingContext.ActionArguments,
                            _actionExecutingContext.Controller);

                        result = await InvokeActionAsync(_actionExecutingContext);
                    }
                    finally
                    {
                        _diagnosticSource.AfterActionMethod(
                            ActionContext,
                            _actionExecutingContext.ActionArguments,
                            _actionExecutingContext.Controller,
                            result);
                    }

                    _actionExecutedContext = new ActionExecutedContext(
                        _actionExecutingContext,
                        _filters,
                        Instance)
                    {
                        Result = result
                    };
                }
            }
            catch (Exception exception)
            {
                // Exceptions thrown by the action method OR filters bubble back up through ActionExcecutedContext.
                _actionExecutedContext = new ActionExecutedContext(
                    _actionExecutingContext,
                    _filters,
                    Instance)
                {
                    ExceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception)
                };
            }
            return _actionExecutedContext;
        }

        private struct FilterCursor
        {
            private int _index;
            private readonly IFilterMetadata[] _filters;

            public FilterCursor(int index, IFilterMetadata[] filters)
            {
                _index = index;
                _filters = filters;
            }

            public FilterCursor(IFilterMetadata[] filters)
            {
                _index = 0;
                _filters = filters;
            }

            public void Reset()
            {
                _index = 0;
            }

            public FilterCursorItem<TFilter, TFilterAsync> GetNextFilter<TFilter, TFilterAsync>()
                where TFilter : class
                where TFilterAsync : class
            {
                while (_index < _filters.Length)
                {
                    var filter = _filters[_index] as TFilter;
                    var filterAsync = _filters[_index] as TFilterAsync;

                    _index += 1;

                    if (filter != null || filterAsync != null)
                    {
                        return new FilterCursorItem<TFilter, TFilterAsync>(_index, filter, filterAsync);
                    }
                }

                return default(FilterCursorItem<TFilter, TFilterAsync>);
            }
        }

        private struct FilterCursorItem<TFilter, TFilterAsync>
        {
            public readonly int Index;
            public readonly TFilter Filter;
            public readonly TFilterAsync FilterAsync;

            public FilterCursorItem(int index, TFilter filter, TFilterAsync filterAsync)
            {
                Index = index;
                Filter = filter;
                FilterAsync = filterAsync;
            }
        }


    }


}
