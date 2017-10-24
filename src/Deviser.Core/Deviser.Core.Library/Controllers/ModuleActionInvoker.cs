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
    public class ModuleActionInvoker //: ControllerActionInvoker, IModuleActionInvoker
    {
        //private readonly IControllerFactory _controllerFactory;
        //private readonly IControllerArgumentBinder _controllerArgumentBinder;
        //private readonly ControllerActionInvokerCache _controllerActionInvokerCache;
        //private readonly DiagnosticSource _diagnosticSource;

        //private readonly ControllerContext _controllerContext;
        //private readonly IFilterMetadata[] _filters;
        //private readonly ObjectMethodExecutor _executor;
        
        //private const string ActionFilterShortCircuitLogMessage =
        //    "Request was short circuited at action filter '{ActionFilter}'.";
        
        //public ModuleActionInvoker(
        //    ControllerActionInvokerCache cache,
        //    IControllerFactory controllerFactory,
        //    IControllerArgumentBinder controllerArgumentBinder,
        //    ILogger logger,
        //    DiagnosticSource diagnosticSource,
        //    ActionContext actionContext,
        //    IReadOnlyList<IValueProviderFactory> valueProviderFactories,
        //    int maxModelValidationErrors)
        //    : base(
        //          cache,
        //          controllerFactory,
        //          controllerArgumentBinder,
        //          logger,
        //          diagnosticSource,
        //          actionContext,
        //          valueProviderFactories,
        //          maxModelValidationErrors)
        //{
        //    //_filterProviders = filterProviders;
        //    _controllerArgumentBinder = controllerArgumentBinder;
        //    _controllerActionInvokerCache = cache;
        //    _diagnosticSource = diagnosticSource;

        //    _controllerFactory = controllerFactory;
        //    _controllerContext = new ControllerContext(actionContext);

        //    var cacheEntry = cache.GetState(_controllerContext);
        //    _filters = cacheEntry.Filters;
        //    _executor = cacheEntry.ActionMethodExecutor;
        //}


        ///// <summary>
        ///// This is method is based on Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> InvokeAction()
        //{
        //    var _controller = _controllerFactory.CreateController(_controllerContext);
        //    var _actionArguments = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        //    //await _controllerArgumentBinder.BindArgumentsAsync(_controllerContext, _controller, arguments);
        //    //var _actionExecutingContext = new ActionExecutingContext(_controllerContext, _filters, arguments, _controller);

        //    IActionResult result = null;

        //    try
        //    {
        //        _diagnosticSource.BeforeActionMethod(
        //            _controllerContext,
        //            _actionArguments,
        //            _controller);

        //        var actionMethodInfo = _controllerContext.ActionDescriptor.MethodInfo;

        //        var arguments = ControllerActionExecutor.PrepareArguments(
        //                    _actionArguments,
        //                    _executor);
                
        //        var returnType = _executor.MethodReturnType;

        //        if (returnType == typeof(void))
        //        {
        //            _executor.Execute(_controller, arguments);
        //            result = new EmptyResult();
        //        }
        //        else if (returnType == typeof(Task))
        //        {
        //            await (Task)_executor.Execute(_controller, arguments);
        //            result = new EmptyResult();
        //        }
        //        else if (_executor.TaskGenericType == typeof(IActionResult))
        //        {
        //            result = await (Task<IActionResult>)_executor.Execute(_controller, arguments);
        //            if (result == null)
        //            {
        //                throw new InvalidOperationException(
        //                    Resources.FormatActionResult_ActionReturnValueCannotBeNull(typeof(IActionResult)));
        //            }
        //        }
        //        else if (_executor.IsTypeAssignableFromIActionResult)
        //        {
        //            if (_executor.IsMethodAsync)
        //            {
        //                result = (IActionResult)await _executor.ExecuteAsync(_controller, arguments);
        //            }
        //            else
        //            {
        //                result = (IActionResult)_executor.Execute(_controller, arguments);
        //            }

        //            if (result == null)
        //            {
        //                throw new InvalidOperationException(
        //                    Resources.FormatActionResult_ActionReturnValueCannotBeNull(_executor.TaskGenericType ?? returnType));
        //            }
        //        }
        //        else if (!_executor.IsMethodAsync)
        //        {
        //            var resultAsObject = _executor.Execute(_controller, arguments);
        //            result = new ObjectResult(resultAsObject)
        //            {
        //                DeclaredType = returnType,
        //            };
        //        }
        //        else if (_executor.TaskGenericType != null)
        //        {
        //            var resultAsObject = await _executor.ExecuteAsync(_controller, arguments);
        //            result = new ObjectResult(resultAsObject)
        //            {
        //                DeclaredType = _executor.TaskGenericType,
        //            };
        //        }
        //        else
        //        {
        //            // This will be the case for types which have derived from Task and Task<T> or non Task types.
        //            throw new InvalidOperationException(Resources.FormatActionExecutor_UnexpectedTaskInstance(
        //                _executor.MethodInfo.Name,
        //                _executor.MethodInfo.DeclaringType));
        //        }
        //    }
        //    finally
        //    {
        //        _diagnosticSource.AfterActionMethod(
        //            _controllerContext,
        //            _actionArguments,
        //            _controller,
        //            result);
        //    }
        //    return result;
        //}
    }


}
