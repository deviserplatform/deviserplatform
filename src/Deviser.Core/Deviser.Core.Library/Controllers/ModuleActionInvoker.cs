using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Controllers;

using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Internal;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleActionInvoker : ControllerActionInvoker, IModuleActionInvoker
    {
        private static readonly IFilterMetadata[] EmptyFilterArray = new IFilterMetadata[0];        
        //private readonly IReadOnlyList<IFilterProvider> _filterProviders;
        ControllerActionInvokerCache _controllerActionInvokerCache;
        private ActionExecutingContext _actionExecutingContext;
        private ActionExecutedContext _actionExecutedContext;
        private readonly IControllerActionArgumentBinder _argumentBinder;
        private readonly ControllerActionDescriptor _descriptor;

        private IFilterMetadata[] _filters;
        private ObjectMethodExecutor _controllerActionMethodExecutor;
        //private FilterCursor _cursor;

        DiagnosticSource _diagnosticSource;

        private const string ActionFilterShortCircuitLogMessage =
            "Request was short circuited at action filter '{ActionFilter}'.";

        protected new object Instance { get; private set; }

        public ModuleActionInvoker(
            ActionContext actionContext,
            ControllerActionInvokerCache controllerActionInvokerCache,
            IControllerFactory controllerFactory,
            ControllerActionDescriptor descriptor,
            IReadOnlyList<IInputFormatter> inputFormatters,
            IControllerActionArgumentBinder argumentBinder,
            IReadOnlyList<IModelValidatorProvider> modelValidatorProviders,
            IReadOnlyList<IValueProviderFactory> valueProviderFactories,
            ILogger logger,
            DiagnosticSource diagnosticSource,
            int maxModelValidationErrors)
            : base(
                  actionContext,
                  controllerActionInvokerCache,
                  controllerFactory,                  
                  descriptor,
                  inputFormatters,
                  argumentBinder,
                  modelValidatorProviders,
                  valueProviderFactories,
                  logger,
                  diagnosticSource,
                  maxModelValidationErrors)
        {
            //_filterProviders = filterProviders;
            _argumentBinder = argumentBinder;
            _controllerActionInvokerCache = controllerActionInvokerCache;
            _diagnosticSource = diagnosticSource;
            _descriptor = descriptor;
        }

        public async Task<IActionResult> InvokeAction()
        {
            var controllerActionInvokerState = _controllerActionInvokerCache.GetState(Context);
            _filters = controllerActionInvokerState.Filters;
            _controllerActionMethodExecutor = controllerActionInvokerState.ActionMethodExecutor;
            //_cursor = new FilterCursor(_filters);

            Instance = CreateInstance();

            var actionArguments = await _argumentBinder.BindActionArgumentsAsync(Context, Instance);

            _actionExecutingContext = new ActionExecutingContext(
                Context,
                _filters,
                actionArguments,
                Instance);

            //var result = await InvokeActionAsync(_actionExecutingContext);

            var actionMethodInfo = _descriptor.MethodInfo;

            var methodExecutor = _controllerActionMethodExecutor;

            var arguments = ControllerActionExecutor.PrepareArguments(
                _actionExecutingContext.ActionArguments,
                actionMethodInfo.GetParameters());

            Logger.ActionMethodExecuting(_actionExecutingContext, arguments);

            var actionReturnValue = await ControllerActionExecutor.ExecuteAsync(
                methodExecutor,
                _actionExecutingContext.Controller,
                arguments);

            var actionResult = CreateActionResult(
                actionMethodInfo.ReturnType,
                actionReturnValue);

            Logger.ActionMethodExecuted(_actionExecutingContext, actionResult);

            return actionResult;
        }

        static IActionResult CreateActionResult(Type declaredReturnType, object actionReturnValue)
        {
            if (declaredReturnType == null)
            {
                throw new ArgumentNullException(nameof(declaredReturnType));
            }

            // optimize common path
            var actionResult = actionReturnValue as IActionResult;
            if (actionResult != null)
            {
                return actionResult;
            }

            if (declaredReturnType == typeof(void) ||
                declaredReturnType == typeof(Task))
            {
                return new EmptyResult();
            }

            // Unwrap potential Task<T> types.
            var actualReturnType = GetTaskInnerTypeOrNull(declaredReturnType) ?? declaredReturnType;
            if (actionReturnValue == null &&
                typeof(IActionResult).IsAssignableFrom(actualReturnType))
            {
                throw new InvalidOperationException(
                    Resources.FormatActionResult_ActionReturnValueCannotBeNull(actualReturnType));
            }

            return new ObjectResult(actionReturnValue)
            {
                DeclaredType = actualReturnType
            };


        }

        private static Type GetTaskInnerTypeOrNull(Type type)
        {
            var genericType = ClosedGenericMatcher.ExtractGenericInterface(type, typeof(Task<>));

            return genericType?.GenericTypeArguments[0];
        }
        ////Copied from Microsoft.AspNetCore.Mvc.Internal.FilterActionInvoker.InvokeActionFilterAsync()
        //private async Task<ActionExecutedContext> InvokeActionFilterAsync()
        //{
        //    Debug.Assert(_actionExecutingContext != null);
        //    if (_actionExecutingContext.Result != null)
        //    {
        //        // If we get here, it means that an async filter set a result AND called next(). This is forbidden.
        //        var message = Resources.FormatAsyncActionFilter_InvalidShortCircuit(
        //            typeof(IAsyncActionFilter).Name,
        //            nameof(ActionExecutingContext.Result),
        //            typeof(ActionExecutingContext).Name,
        //            typeof(ActionExecutionDelegate).Name);

        //        throw new InvalidOperationException(message);
        //    }

        //    var item = _cursor.GetNextFilter<IActionFilter, IAsyncActionFilter>();
        //    try
        //    {
        //        if (item.FilterAsync != null)
        //        {
        //            _diagnosticSource.BeforeOnActionExecution(
        //                _actionExecutingContext,
        //                item.FilterAsync);

        //            await item.FilterAsync.OnActionExecutionAsync(_actionExecutingContext, InvokeActionFilterAsync);

        //            _diagnosticSource.AfterOnActionExecution(
        //                _actionExecutingContext.ActionDescriptor,
        //                _actionExecutedContext,
        //                item.FilterAsync);

        //            if (_actionExecutedContext == null)
        //            {
        //                // If we get here then the filter didn't call 'next' indicating a short circuit
        //                Logger.ActionFilterShortCircuited(item.FilterAsync);

        //                _actionExecutedContext = new ActionExecutedContext(
        //                    _actionExecutingContext,
        //                    _filters,
        //                    Instance)
        //                {
        //                    Canceled = true,
        //                    Result = _actionExecutingContext.Result,
        //                };
        //            }
        //        }
        //        else if (item.Filter != null)
        //        {
        //            _diagnosticSource.BeforeOnActionExecuting(
        //                _actionExecutingContext,
        //                item.Filter);

        //            item.Filter.OnActionExecuting(_actionExecutingContext);

        //            _diagnosticSource.AfterOnActionExecuting(
        //                _actionExecutingContext,
        //                item.Filter);

        //            if (_actionExecutingContext.Result != null)
        //            {
        //                // Short-circuited by setting a result.
        //                Logger.ActionFilterShortCircuited(item.Filter);

        //                _actionExecutedContext = new ActionExecutedContext(
        //                    _actionExecutingContext,
        //                    _filters,
        //                    Instance)
        //                {
        //                    Canceled = true,
        //                    Result = _actionExecutingContext.Result,
        //                };
        //            }
        //            else
        //            {
        //                _diagnosticSource.BeforeOnActionExecuted(
        //                    _actionExecutingContext.ActionDescriptor,
        //                    _actionExecutedContext,
        //                    item.Filter);

        //                item.Filter.OnActionExecuted(await InvokeActionFilterAsync());

        //                _diagnosticSource.BeforeOnActionExecuted(
        //                    _actionExecutingContext.ActionDescriptor,
        //                    _actionExecutedContext,
        //                    item.Filter);
        //            }
        //        }
        //        else
        //        {
        //            // All action filters have run, execute the action method.
        //            IActionResult result = null;

        //            try
        //            {
        //                _diagnosticSource.BeforeActionMethod(
        //                    Context,
        //                    _actionExecutingContext.ActionArguments,
        //                    _actionExecutingContext.Controller);

        //                result = await InvokeActionAsync(_actionExecutingContext);
        //            }
        //            finally
        //            {
        //                _diagnosticSource.AfterActionMethod(
        //                    Context,
        //                    _actionExecutingContext.ActionArguments,
        //                    _actionExecutingContext.Controller,
        //                    result);
        //            }

        //            _actionExecutedContext = new ActionExecutedContext(
        //                _actionExecutingContext,
        //                _filters,
        //                Instance)
        //            {
        //                Result = result
        //            };
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        // Exceptions thrown by the action method OR filters bubble back up through ActionExcecutedContext.
        //        _actionExecutedContext = new ActionExecutedContext(
        //            _actionExecutingContext,
        //            _filters,
        //            Instance)
        //        {
        //            ExceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception)
        //        };
        //    }
        //    return _actionExecutedContext;
        //}

        //private struct FilterCursor
        //{
        //    private int _index;
        //    private readonly IFilterMetadata[] _filters;

        //    public FilterCursor(int index, IFilterMetadata[] filters)
        //    {
        //        _index = index;
        //        _filters = filters;
        //    }

        //    public FilterCursor(IFilterMetadata[] filters)
        //    {
        //        _index = 0;
        //        _filters = filters;
        //    }

        //    public void Reset()
        //    {
        //        _index = 0;
        //    }

        //    public FilterCursorItem<TFilter, TFilterAsync> GetNextFilter<TFilter, TFilterAsync>()
        //        where TFilter : class
        //        where TFilterAsync : class
        //    {
        //        while (_index < _filters.Length)
        //        {
        //            var filter = _filters[_index] as TFilter;
        //            var filterAsync = _filters[_index] as TFilterAsync;

        //            _index += 1;

        //            if (filter != null || filterAsync != null)
        //            {
        //                return new FilterCursorItem<TFilter, TFilterAsync>(_index, filter, filterAsync);
        //            }
        //        }

        //        return default(FilterCursorItem<TFilter, TFilterAsync>);
        //    }
        //}

        //private struct FilterCursorItem<TFilter, TFilterAsync>
        //{
        //    public readonly int Index;
        //    public readonly TFilter Filter;
        //    public readonly TFilterAsync FilterAsync;

        //    public FilterCursorItem(int index, TFilter filter, TFilterAsync filterAsync)
        //    {
        //        Index = index;
        //        Filter = filter;
        //        FilterAsync = filterAsync;
        //    }
        //}

    }


}
