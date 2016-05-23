﻿using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Deviser.Core.Library.Controllers
{
    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Microsoft.AspNetCore.Mvc.Core.Resources", typeof(Resources).GetTypeInfo().Assembly);

        /// <summary>
        /// The argument '{0}' is invalid. Media types which match all types or match all subtypes are not supported.
        /// </summary>
        internal static string MatchAllContentTypeIsNotAllowed
        {
            get { return GetString("MatchAllContentTypeIsNotAllowed"); }
        }

        /// <summary>
        /// The argument '{0}' is invalid. Media types which match all types or match all subtypes are not supported.
        /// </summary>
        internal static string FormatMatchAllContentTypeIsNotAllowed(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MatchAllContentTypeIsNotAllowed"), p0);
        }

        /// <summary>
        /// The content-type '{0}' added in the '{1}' property is invalid. Media types which match all types or match all subtypes are not supported.
        /// </summary>
        internal static string ObjectResult_MatchAllContentType
        {
            get { return GetString("ObjectResult_MatchAllContentType"); }
        }

        /// <summary>
        /// The content-type '{0}' added in the '{1}' property is invalid. Media types which match all types or match all subtypes are not supported.
        /// </summary>
        internal static string FormatObjectResult_MatchAllContentType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ObjectResult_MatchAllContentType"), p0, p1);
        }

        /// <summary>
        /// The method '{0}' on type '{1}' returned an instance of '{2}'. Make sure to call Unwrap on the returned value to avoid unobserved faulted Task.
        /// </summary>
        internal static string ActionExecutor_WrappedTaskInstance
        {
            get { return GetString("ActionExecutor_WrappedTaskInstance"); }
        }

        /// <summary>
        /// The method '{0}' on type '{1}' returned an instance of '{2}'. Make sure to call Unwrap on the returned value to avoid unobserved faulted Task.
        /// </summary>
        internal static string FormatActionExecutor_WrappedTaskInstance(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionExecutor_WrappedTaskInstance"), p0, p1, p2);
        }

        /// <summary>
        /// The method '{0}' on type '{1}' returned a Task instance even though it is not an asynchronous method.
        /// </summary>
        internal static string ActionExecutor_UnexpectedTaskInstance
        {
            get { return GetString("ActionExecutor_UnexpectedTaskInstance"); }
        }

        /// <summary>
        /// The method '{0}' on type '{1}' returned a Task instance even though it is not an asynchronous method.
        /// </summary>
        internal static string FormatActionExecutor_UnexpectedTaskInstance(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionExecutor_UnexpectedTaskInstance"), p0, p1);
        }

        /// <summary>
        /// An action invoker could not be created for action '{0}'.
        /// </summary>
        internal static string ActionInvokerFactory_CouldNotCreateInvoker
        {
            get { return GetString("ActionInvokerFactory_CouldNotCreateInvoker"); }
        }

        /// <summary>
        /// An action invoker could not be created for action '{0}'.
        /// </summary>
        internal static string FormatActionInvokerFactory_CouldNotCreateInvoker(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionInvokerFactory_CouldNotCreateInvoker"), p0);
        }

        /// <summary>
        /// The action descriptor must be of type '{0}'.
        /// </summary>
        internal static string ActionDescriptorMustBeBasedOnControllerAction
        {
            get { return GetString("ActionDescriptorMustBeBasedOnControllerAction"); }
        }

        /// <summary>
        /// The action descriptor must be of type '{0}'.
        /// </summary>
        internal static string FormatActionDescriptorMustBeBasedOnControllerAction(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionDescriptorMustBeBasedOnControllerAction"), p0);
        }

        /// <summary>
        /// Value cannot be null or empty.
        /// </summary>
        internal static string ArgumentCannotBeNullOrEmpty
        {
            get { return GetString("ArgumentCannotBeNullOrEmpty"); }
        }

        /// <summary>
        /// Value cannot be null or empty.
        /// </summary>
        internal static string FormatArgumentCannotBeNullOrEmpty()
        {
            return GetString("ArgumentCannotBeNullOrEmpty");
        }

        /// <summary>
        /// The '{0}' property of '{1}' must not be null.
        /// </summary>
        internal static string PropertyOfTypeCannotBeNull
        {
            get { return GetString("PropertyOfTypeCannotBeNull"); }
        }

        /// <summary>
        /// The '{0}' property of '{1}' must not be null.
        /// </summary>
        internal static string FormatPropertyOfTypeCannotBeNull(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("PropertyOfTypeCannotBeNull"), p0, p1);
        }

        /// <summary>
        /// The '{0}' method of type '{1}' cannot return a null value.
        /// </summary>
        internal static string TypeMethodMustReturnNotNullValue
        {
            get { return GetString("TypeMethodMustReturnNotNullValue"); }
        }

        /// <summary>
        /// The '{0}' method of type '{1}' cannot return a null value.
        /// </summary>
        internal static string FormatTypeMethodMustReturnNotNullValue(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TypeMethodMustReturnNotNullValue"), p0, p1);
        }

        /// <summary>
        /// The value '{0}' is invalid.
        /// </summary>
        internal static string ModelBinding_NullValueNotValid
        {
            get { return GetString("ModelBinding_NullValueNotValid"); }
        }

        /// <summary>
        /// The value '{0}' is invalid.
        /// </summary>
        internal static string FormatModelBinding_NullValueNotValid(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinding_NullValueNotValid"), p0);
        }

        /// <summary>
        /// The passed expression of expression node type '{0}' is invalid. Only simple member access expressions for model properties are supported.
        /// </summary>
        internal static string Invalid_IncludePropertyExpression
        {
            get { return GetString("Invalid_IncludePropertyExpression"); }
        }

        /// <summary>
        /// The passed expression of expression node type '{0}' is invalid. Only simple member access expressions for model properties are supported.
        /// </summary>
        internal static string FormatInvalid_IncludePropertyExpression(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Invalid_IncludePropertyExpression"), p0);
        }

        /// <summary>
        /// No route matches the supplied values.
        /// </summary>
        internal static string NoRoutesMatched
        {
            get { return GetString("NoRoutesMatched"); }
        }

        /// <summary>
        /// No route matches the supplied values.
        /// </summary>
        internal static string FormatNoRoutesMatched()
        {
            return GetString("NoRoutesMatched");
        }

        /// <summary>
        /// If an {0} provides a result value by setting the {1} property of {2} to a non-null value, then it cannot call the next filter by invoking {3}.
        /// </summary>
        internal static string AsyncActionFilter_InvalidShortCircuit
        {
            get { return GetString("AsyncActionFilter_InvalidShortCircuit"); }
        }

        /// <summary>
        /// If an {0} provides a result value by setting the {1} property of {2} to a non-null value, then it cannot call the next filter by invoking {3}.
        /// </summary>
        internal static string FormatAsyncActionFilter_InvalidShortCircuit(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AsyncActionFilter_InvalidShortCircuit"), p0, p1, p2, p3);
        }

        /// <summary>
        /// If an {0} cancels execution by setting the {1} property of {2} to 'true', then it cannot call the next filter by invoking {3}.
        /// </summary>
        internal static string AsyncResultFilter_InvalidShortCircuit
        {
            get { return GetString("AsyncResultFilter_InvalidShortCircuit"); }
        }

        /// <summary>
        /// If an {0} cancels execution by setting the {1} property of {2} to 'true', then it cannot call the next filter by invoking {3}.
        /// </summary>
        internal static string FormatAsyncResultFilter_InvalidShortCircuit(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AsyncResultFilter_InvalidShortCircuit"), p0, p1, p2, p3);
        }

        /// <summary>
        /// The type provided to '{0}' must implement '{1}'.
        /// </summary>
        internal static string FilterFactoryAttribute_TypeMustImplementIFilter
        {
            get { return GetString("FilterFactoryAttribute_TypeMustImplementIFilter"); }
        }

        /// <summary>
        /// The type provided to '{0}' must implement '{1}'.
        /// </summary>
        internal static string FormatFilterFactoryAttribute_TypeMustImplementIFilter(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FilterFactoryAttribute_TypeMustImplementIFilter"), p0, p1);
        }

        /// <summary>
        /// Cannot return null from an action method with a return type of '{0}'.
        /// </summary>
        internal static string ActionResult_ActionReturnValueCannotBeNull
        {
            get { return GetString("ActionResult_ActionReturnValueCannotBeNull"); }
        }

        /// <summary>
        /// Cannot return null from an action method with a return type of '{0}'.
        /// </summary>
        internal static string FormatActionResult_ActionReturnValueCannotBeNull(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ActionResult_ActionReturnValueCannotBeNull"), p0);
        }

        /// <summary>
        /// The type '{0}' must derive from '{1}'.
        /// </summary>
        internal static string TypeMustDeriveFromType
        {
            get { return GetString("TypeMustDeriveFromType"); }
        }

        /// <summary>
        /// The type '{0}' must derive from '{1}'.
        /// </summary>
        internal static string FormatTypeMustDeriveFromType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("TypeMustDeriveFromType"), p0, p1);
        }

        /// <summary>
        /// No encoding found for input formatter '{0}'. There must be at least one supported encoding registered in order for the formatter to read content.
        /// </summary>
        internal static string InputFormatterNoEncoding
        {
            get { return GetString("InputFormatterNoEncoding"); }
        }

        /// <summary>
        /// No encoding found for input formatter '{0}'. There must be at least one supported encoding registered in order for the formatter to read content.
        /// </summary>
        internal static string FormatInputFormatterNoEncoding(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InputFormatterNoEncoding"), p0);
        }

        /// <summary>
        /// Unsupported content type '{0}'.
        /// </summary>
        internal static string UnsupportedContentType
        {
            get { return GetString("UnsupportedContentType"); }
        }

        /// <summary>
        /// Unsupported content type '{0}'.
        /// </summary>
        internal static string FormatUnsupportedContentType(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnsupportedContentType"), p0);
        }

        /// <summary>
        /// No supported media type registered for output formatter '{0}'. There must be at least one supported media type registered in order for the output formatter to write content.
        /// </summary>
        internal static string OutputFormatterNoMediaType
        {
            get { return GetString("OutputFormatterNoMediaType"); }
        }

        /// <summary>
        /// No supported media type registered for output formatter '{0}'. There must be at least one supported media type registered in order for the output formatter to write content.
        /// </summary>
        internal static string FormatOutputFormatterNoMediaType(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("OutputFormatterNoMediaType"), p0);
        }

        /// <summary>
        /// The following errors occurred with attribute routing information:{0}{0}{1}
        /// </summary>
        internal static string AttributeRoute_AggregateErrorMessage
        {
            get { return GetString("AttributeRoute_AggregateErrorMessage"); }
        }

        /// <summary>
        /// The following errors occurred with attribute routing information:{0}{0}{1}
        /// </summary>
        internal static string FormatAttributeRoute_AggregateErrorMessage(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_AggregateErrorMessage"), p0, p1);
        }

        /// <summary>
        /// The attribute route '{0}' cannot contain a parameter named '{{{1}}}'. Use '[{1}]' in the route template to insert the value '{2}'.
        /// </summary>
        internal static string AttributeRoute_CannotContainParameter
        {
            get { return GetString("AttributeRoute_CannotContainParameter"); }
        }

        /// <summary>
        /// The attribute route '{0}' cannot contain a parameter named '{{{1}}}'. Use '[{1}]' in the route template to insert the value '{2}'.
        /// </summary>
        internal static string FormatAttributeRoute_CannotContainParameter(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_CannotContainParameter"), p0, p1, p2);
        }

        /// <summary>
        /// For action: '{0}'{1}Error: {2}
        /// </summary>
        internal static string AttributeRoute_IndividualErrorMessage
        {
            get { return GetString("AttributeRoute_IndividualErrorMessage"); }
        }

        /// <summary>
        /// For action: '{0}'{1}Error: {2}
        /// </summary>
        internal static string FormatAttributeRoute_IndividualErrorMessage(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_IndividualErrorMessage"), p0, p1, p2);
        }

        /// <summary>
        /// An empty replacement token ('[]') is not allowed.
        /// </summary>
        internal static string AttributeRoute_TokenReplacement_EmptyTokenNotAllowed
        {
            get { return GetString("AttributeRoute_TokenReplacement_EmptyTokenNotAllowed"); }
        }

        /// <summary>
        /// An empty replacement token ('[]') is not allowed.
        /// </summary>
        internal static string FormatAttributeRoute_TokenReplacement_EmptyTokenNotAllowed()
        {
            return GetString("AttributeRoute_TokenReplacement_EmptyTokenNotAllowed");
        }

        /// <summary>
        /// Token delimiters ('[', ']') are imbalanced.
        /// </summary>
        internal static string AttributeRoute_TokenReplacement_ImbalancedSquareBrackets
        {
            get { return GetString("AttributeRoute_TokenReplacement_ImbalancedSquareBrackets"); }
        }

        /// <summary>
        /// Token delimiters ('[', ']') are imbalanced.
        /// </summary>
        internal static string FormatAttributeRoute_TokenReplacement_ImbalancedSquareBrackets()
        {
            return GetString("AttributeRoute_TokenReplacement_ImbalancedSquareBrackets");
        }

        /// <summary>
        /// The route template '{0}' has invalid syntax. {1}
        /// </summary>
        internal static string AttributeRoute_TokenReplacement_InvalidSyntax
        {
            get { return GetString("AttributeRoute_TokenReplacement_InvalidSyntax"); }
        }

        /// <summary>
        /// The route template '{0}' has invalid syntax. {1}
        /// </summary>
        internal static string FormatAttributeRoute_TokenReplacement_InvalidSyntax(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_TokenReplacement_InvalidSyntax"), p0, p1);
        }

        /// <summary>
        /// While processing template '{0}', a replacement value for the token '{1}' could not be found. Available tokens: '{2}'.
        /// </summary>
        internal static string AttributeRoute_TokenReplacement_ReplacementValueNotFound
        {
            get { return GetString("AttributeRoute_TokenReplacement_ReplacementValueNotFound"); }
        }

        /// <summary>
        /// While processing template '{0}', a replacement value for the token '{1}' could not be found. Available tokens: '{2}'.
        /// </summary>
        internal static string FormatAttributeRoute_TokenReplacement_ReplacementValueNotFound(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_TokenReplacement_ReplacementValueNotFound"), p0, p1, p2);
        }

        /// <summary>
        /// A replacement token is not closed.
        /// </summary>
        internal static string AttributeRoute_TokenReplacement_UnclosedToken
        {
            get { return GetString("AttributeRoute_TokenReplacement_UnclosedToken"); }
        }

        /// <summary>
        /// A replacement token is not closed.
        /// </summary>
        internal static string FormatAttributeRoute_TokenReplacement_UnclosedToken()
        {
            return GetString("AttributeRoute_TokenReplacement_UnclosedToken");
        }

        /// <summary>
        /// An unescaped '[' token is not allowed inside of a replacement token. Use '[[' to escape.
        /// </summary>
        internal static string AttributeRoute_TokenReplacement_UnescapedBraceInToken
        {
            get { return GetString("AttributeRoute_TokenReplacement_UnescapedBraceInToken"); }
        }

        /// <summary>
        /// An unescaped '[' token is not allowed inside of a replacement token. Use '[[' to escape.
        /// </summary>
        internal static string FormatAttributeRoute_TokenReplacement_UnescapedBraceInToken()
        {
            return GetString("AttributeRoute_TokenReplacement_UnescapedBraceInToken");
        }

        /// <summary>
        /// The value must be either '{0}' or '{1}'.
        /// </summary>
        internal static string RouteConstraintAttribute_InvalidKeyHandlingValue
        {
            get { return GetString("RouteConstraintAttribute_InvalidKeyHandlingValue"); }
        }

        /// <summary>
        /// The value must be either '{0}' or '{1}'.
        /// </summary>
        internal static string FormatRouteConstraintAttribute_InvalidKeyHandlingValue(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("RouteConstraintAttribute_InvalidKeyHandlingValue"), p0, p1);
        }

        /// <summary>
        /// Unable to find the required services. Please add all the required services by calling '{0}' inside the call to '{1}' or '{2}' in the application startup code.
        /// </summary>
        internal static string UnableToFindServices
        {
            get { return GetString("UnableToFindServices"); }
        }

        /// <summary>
        /// Unable to find the required services. Please add all the required services by calling '{0}' inside the call to '{1}' or '{2}' in the application startup code.
        /// </summary>
        internal static string FormatUnableToFindServices(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("UnableToFindServices"), p0, p1, p2);
        }

        /// <summary>
        /// Two or more routes named '{0}' have different templates.
        /// </summary>
        internal static string AttributeRoute_DifferentLinkGenerationEntries_SameName
        {
            get { return GetString("AttributeRoute_DifferentLinkGenerationEntries_SameName"); }
        }

        /// <summary>
        /// Two or more routes named '{0}' have different templates.
        /// </summary>
        internal static string FormatAttributeRoute_DifferentLinkGenerationEntries_SameName(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_DifferentLinkGenerationEntries_SameName"), p0);
        }

        /// <summary>
        /// Action: '{0}' - Template: '{1}'
        /// </summary>
        internal static string AttributeRoute_DuplicateNames_Item
        {
            get { return GetString("AttributeRoute_DuplicateNames_Item"); }
        }

        /// <summary>
        /// Action: '{0}' - Template: '{1}'
        /// </summary>
        internal static string FormatAttributeRoute_DuplicateNames_Item(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_DuplicateNames_Item"), p0, p1);
        }

        /// <summary>
        /// Attribute routes with the same name '{0}' must have the same template:{1}{2}
        /// </summary>
        internal static string AttributeRoute_DuplicateNames
        {
            get { return GetString("AttributeRoute_DuplicateNames"); }
        }

        /// <summary>
        /// Attribute routes with the same name '{0}' must have the same template:{1}{2}
        /// </summary>
        internal static string FormatAttributeRoute_DuplicateNames(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_DuplicateNames"), p0, p1, p2);
        }

        /// <summary>
        /// Error {0}:{1}{2}
        /// </summary>
        internal static string AttributeRoute_AggregateErrorMessage_ErrorNumber
        {
            get { return GetString("AttributeRoute_AggregateErrorMessage_ErrorNumber"); }
        }

        /// <summary>
        /// Error {0}:{1}{2}
        /// </summary>
        internal static string FormatAttributeRoute_AggregateErrorMessage_ErrorNumber(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_AggregateErrorMessage_ErrorNumber"), p0, p1, p2);
        }

        /// <summary>
        /// A method '{0}' must not define attribute routed actions and non attribute routed actions at the same time:{1}{2}{1}{1}Use 'AcceptVerbsAttribute' to create a single route that allows multiple HTTP verbs and defines a route, or set a route template in all attributes that constrain HTTP verbs.
        /// </summary>
        internal static string AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod
        {
            get { return GetString("AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod"); }
        }

        /// <summary>
        /// A method '{0}' must not define attribute routed actions and non attribute routed actions at the same time:{1}{2}{1}{1}Use 'AcceptVerbsAttribute' to create a single route that allows multiple HTTP verbs and defines a route, or set a route template in all attributes that constrain HTTP verbs.
        /// </summary>
        internal static string FormatAttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod"), p0, p1, p2);
        }

        /// <summary>
        /// Action: '{0}' - Route Template: '{1}' - HTTP Verbs: '{2}'
        /// </summary>
        internal static string AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod_Item
        {
            get { return GetString("AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod_Item"); }
        }

        /// <summary>
        /// Action: '{0}' - Route Template: '{1}' - HTTP Verbs: '{2}'
        /// </summary>
        internal static string FormatAttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod_Item(object p0, object p1, object p2)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_MixedAttributeAndConventionallyRoutedActions_ForMethod_Item"), p0, p1, p2);
        }

        /// <summary>
        /// (none)
        /// </summary>
        internal static string AttributeRoute_NullTemplateRepresentation
        {
            get { return GetString("AttributeRoute_NullTemplateRepresentation"); }
        }

        /// <summary>
        /// (none)
        /// </summary>
        internal static string FormatAttributeRoute_NullTemplateRepresentation()
        {
            return GetString("AttributeRoute_NullTemplateRepresentation");
        }

        /// <summary>
        /// Multiple actions matched. The following actions matched route data and had all constraints satisfied:{0}{0}{1}
        /// </summary>
        internal static string DefaultActionSelector_AmbiguousActions
        {
            get { return GetString("DefaultActionSelector_AmbiguousActions"); }
        }

        /// <summary>
        /// Multiple actions matched. The following actions matched route data and had all constraints satisfied:{0}{0}{1}
        /// </summary>
        internal static string FormatDefaultActionSelector_AmbiguousActions(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DefaultActionSelector_AmbiguousActions"), p0, p1);
        }

        /// <summary>
        /// Could not find file: {0}
        /// </summary>
        internal static string FileResult_InvalidPath
        {
            get { return GetString("FileResult_InvalidPath"); }
        }

        /// <summary>
        /// Could not find file: {0}
        /// </summary>
        internal static string FormatFileResult_InvalidPath(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FileResult_InvalidPath"), p0);
        }

        /// <summary>
        /// The input was not valid.
        /// </summary>
        internal static string SerializableError_DefaultError
        {
            get { return GetString("SerializableError_DefaultError"); }
        }

        /// <summary>
        /// The input was not valid.
        /// </summary>
        internal static string FormatSerializableError_DefaultError()
        {
            return GetString("SerializableError_DefaultError");
        }

        /// <summary>
        /// If an {0} provides a result value by setting the {1} property of {2} to a non-null value, then it cannot call the next filter by invoking {3}.
        /// </summary>
        internal static string AsyncResourceFilter_InvalidShortCircuit
        {
            get { return GetString("AsyncResourceFilter_InvalidShortCircuit"); }
        }

        /// <summary>
        /// If an {0} provides a result value by setting the {1} property of {2} to a non-null value, then it cannot call the next filter by invoking {3}.
        /// </summary>
        internal static string FormatAsyncResourceFilter_InvalidShortCircuit(object p0, object p1, object p2, object p3)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("AsyncResourceFilter_InvalidShortCircuit"), p0, p1, p2, p3);
        }

        /// <summary>
        /// If the '{0}' property is not set to true, '{1}' property must be specified.
        /// </summary>
        internal static string ResponseCache_SpecifyDuration
        {
            get { return GetString("ResponseCache_SpecifyDuration"); }
        }

        /// <summary>
        /// If the '{0}' property is not set to true, '{1}' property must be specified.
        /// </summary>
        internal static string FormatResponseCache_SpecifyDuration(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ResponseCache_SpecifyDuration"), p0, p1);
        }

        /// <summary>
        /// The action '{0}' has ApiExplorer enabled, but is using conventional routing. Only actions which use attribute routing support ApiExplorer.
        /// </summary>
        internal static string ApiExplorer_UnsupportedAction
        {
            get { return GetString("ApiExplorer_UnsupportedAction"); }
        }

        /// <summary>
        /// The action '{0}' has ApiExplorer enabled, but is using conventional routing. Only actions which use attribute routing support ApiExplorer.
        /// </summary>
        internal static string FormatApiExplorer_UnsupportedAction(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ApiExplorer_UnsupportedAction"), p0);
        }

        /// <summary>
        /// The media type "{0}" is not valid. MediaTypes containing wildcards (*) are not allowed in formatter mappings.
        /// </summary>
        internal static string FormatterMappings_NotValidMediaType
        {
            get { return GetString("FormatterMappings_NotValidMediaType"); }
        }

        /// <summary>
        /// The media type "{0}" is not valid. MediaTypes containing wildcards (*) are not allowed in formatter mappings.
        /// </summary>
        internal static string FormatFormatterMappings_NotValidMediaType(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FormatterMappings_NotValidMediaType"), p0);
        }

        /// <summary>
        /// The format provided is invalid '{0}'. A format must be a non-empty file-extension, optionally prefixed with a '.' character.
        /// </summary>
        internal static string Format_NotValid
        {
            get { return GetString("Format_NotValid"); }
        }

        /// <summary>
        /// The format provided is invalid '{0}'. A format must be a non-empty file-extension, optionally prefixed with a '.' character.
        /// </summary>
        internal static string FormatFormat_NotValid(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Format_NotValid"), p0);
        }

        /// <summary>
        /// The '{0}' cache profile is not defined.
        /// </summary>
        internal static string CacheProfileNotFound
        {
            get { return GetString("CacheProfileNotFound"); }
        }

        /// <summary>
        /// The '{0}' cache profile is not defined.
        /// </summary>
        internal static string FormatCacheProfileNotFound(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CacheProfileNotFound"), p0);
        }

        /// <summary>
        /// The model's runtime type '{0}' is not assignable to the type '{1}'.
        /// </summary>
        internal static string ModelType_WrongType
        {
            get { return GetString("ModelType_WrongType"); }
        }

        /// <summary>
        /// The model's runtime type '{0}' is not assignable to the type '{1}'.
        /// </summary>
        internal static string FormatModelType_WrongType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelType_WrongType"), p0, p1);
        }

        /// <summary>
        /// The type '{0}' cannot be activated by '{1}' because it is either a value type, an interface, an abstract class or an open generic type.
        /// </summary>
        internal static string ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated
        {
            get { return GetString("ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated"); }
        }

        /// <summary>
        /// The type '{0}' cannot be activated by '{1}' because it is either a value type, an interface, an abstract class or an open generic type.
        /// </summary>
        internal static string FormatValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ValueInterfaceAbstractOrOpenGenericTypesCannotBeActivated"), p0, p1);
        }

        /// <summary>
        /// The type '{0}' must implement '{1}' to be used as a model binder.
        /// </summary>
        internal static string BinderType_MustBeIModelBinder
        {
            get { return GetString("BinderType_MustBeIModelBinder"); }
        }

        /// <summary>
        /// The type '{0}' must implement '{1}' to be used as a model binder.
        /// </summary>
        internal static string FormatBinderType_MustBeIModelBinder(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("BinderType_MustBeIModelBinder"), p0, p1);
        }

        /// <summary>
        /// The provided binding source '{0}' is a composite. '{1}' requires that the source must represent a single type of input.
        /// </summary>
        internal static string BindingSource_CannotBeComposite
        {
            get { return GetString("BindingSource_CannotBeComposite"); }
        }

        /// <summary>
        /// The provided binding source '{0}' is a composite. '{1}' requires that the source must represent a single type of input.
        /// </summary>
        internal static string FormatBindingSource_CannotBeComposite(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("BindingSource_CannotBeComposite"), p0, p1);
        }

        /// <summary>
        /// The provided binding source '{0}' is a greedy data source. '{1}' does not support greedy data sources.
        /// </summary>
        internal static string BindingSource_CannotBeGreedy
        {
            get { return GetString("BindingSource_CannotBeGreedy"); }
        }

        /// <summary>
        /// The provided binding source '{0}' is a greedy data source. '{1}' does not support greedy data sources.
        /// </summary>
        internal static string FormatBindingSource_CannotBeGreedy(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("BindingSource_CannotBeGreedy"), p0, p1);
        }

        /// <summary>
        /// The property {0}.{1} could not be found.
        /// </summary>
        internal static string Common_PropertyNotFound
        {
            get { return GetString("Common_PropertyNotFound"); }
        }

        /// <summary>
        /// The property {0}.{1} could not be found.
        /// </summary>
        internal static string FormatCommon_PropertyNotFound(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("Common_PropertyNotFound"), p0, p1);
        }

        /// <summary>
        /// The key '{0}' is invalid JQuery syntax because it is missing a closing bracket.
        /// </summary>
        internal static string JQueryFormValueProviderFactory_MissingClosingBracket
        {
            get { return GetString("JQueryFormValueProviderFactory_MissingClosingBracket"); }
        }

        /// <summary>
        /// The key '{0}' is invalid JQuery syntax because it is missing a closing bracket.
        /// </summary>
        internal static string FormatJQueryFormValueProviderFactory_MissingClosingBracket(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("JQueryFormValueProviderFactory_MissingClosingBracket"), p0);
        }

        /// <summary>
        /// A value is required.
        /// </summary>
        internal static string KeyValuePair_BothKeyAndValueMustBePresent
        {
            get { return GetString("KeyValuePair_BothKeyAndValueMustBePresent"); }
        }

        /// <summary>
        /// A value is required.
        /// </summary>
        internal static string FormatKeyValuePair_BothKeyAndValueMustBePresent()
        {
            return GetString("KeyValuePair_BothKeyAndValueMustBePresent");
        }

        /// <summary>
        /// The binding context has a null Model, but this binder requires a non-null model of type '{0}'.
        /// </summary>
        internal static string ModelBinderUtil_ModelCannotBeNull
        {
            get { return GetString("ModelBinderUtil_ModelCannotBeNull"); }
        }

        /// <summary>
        /// The binding context has a null Model, but this binder requires a non-null model of type '{0}'.
        /// </summary>
        internal static string FormatModelBinderUtil_ModelCannotBeNull(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinderUtil_ModelCannotBeNull"), p0);
        }

        /// <summary>
        /// The binding context has a Model of type '{0}', but this binder can only operate on models of type '{1}'.
        /// </summary>
        internal static string ModelBinderUtil_ModelInstanceIsWrong
        {
            get { return GetString("ModelBinderUtil_ModelInstanceIsWrong"); }
        }

        /// <summary>
        /// The binding context has a Model of type '{0}', but this binder can only operate on models of type '{1}'.
        /// </summary>
        internal static string FormatModelBinderUtil_ModelInstanceIsWrong(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinderUtil_ModelInstanceIsWrong"), p0, p1);
        }

        /// <summary>
        /// The binding context cannot have a null ModelMetadata.
        /// </summary>
        internal static string ModelBinderUtil_ModelMetadataCannotBeNull
        {
            get { return GetString("ModelBinderUtil_ModelMetadataCannotBeNull"); }
        }

        /// <summary>
        /// The binding context cannot have a null ModelMetadata.
        /// </summary>
        internal static string FormatModelBinderUtil_ModelMetadataCannotBeNull()
        {
            return GetString("ModelBinderUtil_ModelMetadataCannotBeNull");
        }

        /// <summary>
        /// The binding context has a ModelType of '{0}', but this binder can only operate on models of type '{1}'.
        /// </summary>
        internal static string ModelBinderUtil_ModelTypeIsWrong
        {
            get { return GetString("ModelBinderUtil_ModelTypeIsWrong"); }
        }

        /// <summary>
        /// The binding context has a ModelType of '{0}', but this binder can only operate on models of type '{1}'.
        /// </summary>
        internal static string FormatModelBinderUtil_ModelTypeIsWrong(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinderUtil_ModelTypeIsWrong"), p0, p1);
        }

        /// <summary>
        /// A value for the '{0}' property was not provided.
        /// </summary>
        internal static string ModelBinding_MissingBindRequiredMember
        {
            get { return GetString("ModelBinding_MissingBindRequiredMember"); }
        }

        /// <summary>
        /// A value for the '{0}' property was not provided.
        /// </summary>
        internal static string FormatModelBinding_MissingBindRequiredMember(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ModelBinding_MissingBindRequiredMember"), p0);
        }

        /// <summary>
        /// The type '{0}' does not implement the interface '{1}'.
        /// </summary>
        internal static string PropertyBindingPredicateProvider_WrongType
        {
            get { return GetString("PropertyBindingPredicateProvider_WrongType"); }
        }

        /// <summary>
        /// The type '{0}' does not implement the interface '{1}'.
        /// </summary>
        internal static string FormatPropertyBindingPredicateProvider_WrongType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("PropertyBindingPredicateProvider_WrongType"), p0, p1);
        }

        /// <summary>
        /// The parameter conversion from type '{0}' to type '{1}' failed because no type converter can convert between these types.
        /// </summary>
        internal static string ValueProviderResult_NoConverterExists
        {
            get { return GetString("ValueProviderResult_NoConverterExists"); }
        }

        /// <summary>
        /// The parameter conversion from type '{0}' to type '{1}' failed because no type converter can convert between these types.
        /// </summary>
        internal static string FormatValueProviderResult_NoConverterExists(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ValueProviderResult_NoConverterExists"), p0, p1);
        }

        /// <summary>
        /// The byte buffer must have a length of at least '{0}' to be used with a char buffer of size '{1}' and encoding '{2}'. Use '{3}.{4}' to compute the correct size for the byte buffer.
        /// </summary>
        internal static string HttpResponseStreamWriter_InvalidBufferSize
        {
            get { return GetString("HttpResponseStreamWriter_InvalidBufferSize"); }
        }

        /// <summary>
        /// The byte buffer must have a length of at least '{0}' to be used with a char buffer of size '{1}' and encoding '{2}'. Use '{3}.{4}' to compute the correct size for the byte buffer.
        /// </summary>
        internal static string FormatHttpResponseStreamWriter_InvalidBufferSize(object p0, object p1, object p2, object p3, object p4)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HttpResponseStreamWriter_InvalidBufferSize"), p0, p1, p2, p3, p4);
        }

        /// <summary>
        /// Path '{0}' was not rooted.
        /// </summary>
        internal static string FileResult_PathNotRooted
        {
            get { return GetString("FileResult_PathNotRooted"); }
        }

        /// <summary>
        /// Path '{0}' was not rooted.
        /// </summary>
        internal static string FormatFileResult_PathNotRooted(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FileResult_PathNotRooted"), p0);
        }

        /// <summary>
        /// The supplied URL is not local. A URL with an absolute path is considered local if it does not have a host/authority part. URLs using virtual paths ('~/') are also local.
        /// </summary>
        internal static string UrlNotLocal
        {
            get { return GetString("UrlNotLocal"); }
        }

        /// <summary>
        /// The supplied URL is not local. A URL with an absolute path is considered local if it does not have a host/authority part. URLs using virtual paths ('~/') are also local.
        /// </summary>
        internal static string FormatUrlNotLocal()
        {
            return GetString("UrlNotLocal");
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}
