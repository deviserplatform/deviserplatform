using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;

namespace Deviser.Core.Library.Controllers
{
    //Based on Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvokerProvider

    public class ModuleInvokerProvider : IModuleInvokerProvider
    {
        private readonly IControllerActionArgumentBinder _argumentBinder;
        private readonly IControllerFactory _controllerFactory;
        private readonly ControllerActionInvokerCache _controllerActionInvokerCache;
        private readonly IReadOnlyList<IInputFormatter> _inputFormatters;
        private readonly IReadOnlyList<IModelValidatorProvider> _modelValidatorProviders;
        private readonly IReadOnlyList<IValueProviderFactory> _valueProviderFactories;
        private readonly int _maxModelValidationErrors;
        private readonly ILogger _logger;
        private readonly DiagnosticSource _diagnosticSource;

        public ModuleInvokerProvider(
            IControllerFactory controllerFactory,
            ControllerActionInvokerCache controllerActionInvokerCache,
            IControllerActionArgumentBinder argumentBinder,
            IOptions<MvcOptions> optionsAccessor,
            ILoggerFactory loggerFactory,
            DiagnosticSource diagnosticSource)
        {
            _controllerFactory = controllerFactory;
            _controllerActionInvokerCache = controllerActionInvokerCache;
            _argumentBinder = argumentBinder;
            _inputFormatters = optionsAccessor.Value.InputFormatters.ToArray();
            _modelValidatorProviders = optionsAccessor.Value.ModelValidatorProviders.ToArray();
            _valueProviderFactories = optionsAccessor.Value.ValueProviderFactories.ToArray();
            _maxModelValidationErrors = optionsAccessor.Value.MaxModelValidationErrors;
            _logger = loggerFactory.CreateLogger<ModuleActionInvoker>();
            _diagnosticSource = diagnosticSource;
        }

        public int Order
        {
            get { return -1000; }
        }

        /// <inheritdoc />
        public IModuleActionInvoker CreateInvoker(ActionContext actionContext, ControllerActionDescriptor descriptor)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var actionDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;

            if (actionDescriptor != null)
            {
                var result = new ModuleActionInvoker(
                    actionContext,
                  _controllerActionInvokerCache,
                    _controllerFactory,
                    actionDescriptor,
                    _inputFormatters,
                    _argumentBinder,
                    _modelValidatorProviders,
                    _valueProviderFactories,
                    _logger,
                    _diagnosticSource,
                    _maxModelValidationErrors);

                return result;
            }

            return null;
        }
    }
}
