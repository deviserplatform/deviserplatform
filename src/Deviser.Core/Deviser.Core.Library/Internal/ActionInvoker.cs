using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Deviser.Core.Common.Internal;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Deviser.Core.Library.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Deviser.Core.Common.Extensions;
using DefaultAssemblyPartDiscoveryProvider = Deviser.Core.Common.Internal.DefaultAssemblyPartDiscoveryProvider;

namespace Deviser.Core.Library.Internal
{
    public class ActionInvoker : IActionInvoker
    {
        private readonly ObjectMethodExecutorCache _cache;
        private readonly ITypeActivatorCache _typeActivatorCache;
        private readonly IControllerPropertyActivator[] _propertyActivators;
        private readonly List<TypeInfo> _allControllers;

        private const string ControllerTypeNameSuffix = "Controller";
        
        public ActionInvoker(
            ITypeActivatorCache typeActivatorCache,
            ObjectMethodExecutorCache cache,
            IEnumerable<IControllerPropertyActivator> propertyActivators)
        {
            _typeActivatorCache = typeActivatorCache ?? throw new ArgumentNullException(nameof(typeActivatorCache));
            _propertyActivators = propertyActivators.ToArray();
            _cache = cache;


            var assemblies = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Globals.ApplicationEntryPoint);
            _allControllers = new List<TypeInfo>();
            foreach (var assembly in assemblies)
            {
                var controllerTypes = assembly.DefinedTypes.Where(t => IsController(t)).ToList();

                if (controllerTypes != null && controllerTypes.Count > 0)
                    _allControllers.AddRange(controllerTypes);
            }
        }

        public async Task<IActionResult> InvokeAction(HttpContext httpContext, ModuleAction moduleAction, ActionContext actionContext)
        {            
            return await InvokeAction(httpContext, moduleAction.ControllerNamespace, moduleAction.ControllerName, moduleAction.ActionName, actionContext);
        }

        public async Task<IActionResult> InvokeAction(HttpContext httpContext, string controllerNamespace, string controllerName, string actionName, ActionContext actionContext)
        {
            IActionResult result = null;

            var targetController = _allControllers.FirstOrDefault(c => c.Namespace == controllerNamespace && c.Name == controllerName + ControllerTypeNameSuffix);

            if (targetController == null)
                throw new Exception("Controller not found");

            var targetAction = targetController.GetMethods().FirstOrDefault(m => m.Name == actionName);

            if (targetAction == null)
                throw new Exception("Action not found");

            var executor = _cache.GetExecutor(targetAction, targetController);

            var actionArguments = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (var param in actionContext.ActionDescriptor.Parameters)
            {
                if (actionContext.RouteData.Values.ContainsKey(param.Name))
                {
                    actionArguments.Add(param.Name, actionContext.RouteData.Values[param.Name]);
                }
            }

            var arguments = PrepareArguments(actionArguments, executor);

            var returnType = executor.MethodReturnType;

            var controllerContext = new ControllerContext(actionContext);
            //var controller1 = _controllerFactory.CreateController(controllerContext);

            var serviceProvider = httpContext.RequestServices;
            var controller = _typeActivatorCache.CreateInstance<object>(serviceProvider, targetController.AsType()); //Returns 

            foreach (var propertyActivator in _propertyActivators)
            {
                propertyActivator.Activate(controllerContext, controller);
            }

            //((Deviser.Core.Library.Controllers.DeviserController)controller).TempData = new TempDataDictionary()

            if (returnType == typeof(void))
            {
                executor.Execute(controller, arguments);
                result = new EmptyResult();
            }
            else if (returnType == typeof(Task))
            {
                await (Task)executor.Execute(controller, arguments);
                result = new EmptyResult();
            }
            else if (executor.TaskGenericType == typeof(IActionResult))
            {
                result = await (Task<IActionResult>)executor.Execute(controller, arguments);
                if (result == null)
                {
                    throw new InvalidOperationException(
                        Resources.FormatActionResult_ActionReturnValueCannotBeNull(typeof(IActionResult)));
                }
            }
            else if (executor.IsTypeAssignableFromIActionResult)
            {
                if (executor.IsMethodAsync)
                {
                    result = (IActionResult)await executor.ExecuteAsync(controller, arguments);
                }
                else
                {
                    result = (IActionResult)executor.Execute(controller, arguments);
                }

                if (result == null)
                {
                    throw new InvalidOperationException(
                        Resources.FormatActionResult_ActionReturnValueCannotBeNull(executor.TaskGenericType ?? returnType));
                }
            }
            else if (!executor.IsMethodAsync)
            {
                var resultAsObject = executor.Execute(controller, arguments);
                result = new ObjectResult(resultAsObject)
                {
                    DeclaredType = returnType,
                };
            }
            else if (executor.TaskGenericType != null)
            {
                var resultAsObject = await executor.ExecuteAsync(controller, arguments);
                result = new ObjectResult(resultAsObject)
                {
                    DeclaredType = executor.TaskGenericType,
                };
            }
            else
            {
                // This will be the case for types which have derived from Task and Task<T> or non Task types.
                throw new InvalidOperationException(Resources.FormatActionExecutor_UnexpectedTaskInstance(
                    executor.MethodInfo.Name,
                    executor.MethodInfo.DeclaringType));
            }

            ((IDisposable)controller).RegisterForDispose(httpContext);

            return result;
        }

        private bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            // We only consider public top-level classes as controllers. IsPublic returns false for nested
            // classes, regardless of visibility modifiers
            if (!typeInfo.IsPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase) &&
                !typeInfo.IsDefined(typeof(ControllerAttribute)))
            {
                return false;
            }

            return true;
        }

        private object[] PrepareArguments(
           IDictionary<string, object> actionParameters,
           ObjectMethodExecutor actionMethodExecutor)
        {
            var declaredParameterInfos = actionMethodExecutor.ActionParameters;
            var count = declaredParameterInfos.Length;
            if (count == 0)
            {
                return null;
            }

            var arguments = new object[count];
            for (var index = 0; index < count; index++)
            {
                var parameterInfo = declaredParameterInfos[index];
                object value;

                if (!actionParameters.TryGetValue(parameterInfo.Name, out value))
                {
                    value = actionMethodExecutor.GetDefaultValueForParameter(index);
                }

                arguments[index] = value;
            }

            return arguments;
        }

        public void Dispose()
        {
            if(_allControllers!=null)
            {
                _allControllers.GetEnumerator().Dispose();
            }

            _cache.Dispose();
            _typeActivatorCache.Dispose();


        }
    }
}
