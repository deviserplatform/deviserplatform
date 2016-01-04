using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.AspNet.Mvc.Controllers;

namespace Deviser.Core.Library.Controllers
{
    public class ModuleInvokerProvider : IModuleInvokerProvider
    {
        private readonly IControllerActionArgumentBinder _argumentBinder;
        private readonly IControllerFactory _controllerFactory;
        private readonly IFilterProvider[] _filterProviders;
        private readonly IReadOnlyList<IInputFormatter> _inputFormatters;
        private readonly IReadOnlyList<IModelBinder> _modelBinders;
        private readonly IReadOnlyList<IOutputFormatter> _outputFormatters;
        private readonly IReadOnlyList<IModelValidatorProvider> _modelValidatorProviders;
        private readonly IReadOnlyList<IValueProviderFactory> _valueProviderFactories;
        private readonly IActionBindingContextAccessor _actionBindingContextAccessor;
        private readonly int _maxModelValidationErrors;
        private readonly ILogger _logger;
        private readonly DiagnosticSource _diagnosticSource;

        public ModuleInvokerProvider(
            IControllerFactory controllerFactory,
            IEnumerable<IFilterProvider> filterProviders,
            IControllerActionArgumentBinder argumentBinder,
            IOptions<MvcOptions> optionsAccessor,
            IActionBindingContextAccessor actionBindingContextAccessor,
            ILoggerFactory loggerFactory,
            DiagnosticSource diagnosticSource)
        {
            _controllerFactory = controllerFactory;
            _filterProviders = filterProviders.OrderBy(item => item.Order).ToArray();
            _argumentBinder = argumentBinder;
            _inputFormatters = optionsAccessor.Value.InputFormatters.ToArray();
            _outputFormatters = optionsAccessor.Value.OutputFormatters.ToArray();
            _modelBinders = optionsAccessor.Value.ModelBinders.ToArray();
            _modelValidatorProviders = optionsAccessor.Value.ModelValidatorProviders.ToArray();
            _valueProviderFactories = optionsAccessor.Value.ValueProviderFactories.ToArray();
            _actionBindingContextAccessor = actionBindingContextAccessor;
            _maxModelValidationErrors = optionsAccessor.Value.MaxModelValidationErrors;
            _logger = loggerFactory.CreateLogger<Microsoft.AspNet.Mvc.Controllers.ControllerActionInvoker>();
            _diagnosticSource = diagnosticSource;
        }

        public int Order
        {
            get { return -1000; }
        }

        /// <inheritdoc />
        public IActionInvoker CreateInvoker(ActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var actionDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;

            if (actionDescriptor != null)
            {
                var result = new ControllerActionInvoker(
                    actionContext,
                    _filterProviders,
                    _controllerFactory,
                    actionDescriptor,
                    _inputFormatters,
                    _outputFormatters,
                    _argumentBinder,
                    _modelBinders,
                    _modelValidatorProviders,
                    _valueProviderFactories,
                    _actionBindingContextAccessor,
                    _logger,
                    _diagnosticSource,
                    _maxModelValidationErrors);

                return result;
            }

            return null;
        }
    }
}
