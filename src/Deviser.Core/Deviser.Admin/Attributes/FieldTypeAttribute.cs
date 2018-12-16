using Deviser.Core.Common.DomainTypes.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Deviser.Admin.Attributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class FieldTypeAttribute : ValidationAttribute
    {
        private static readonly string[] _fieldTypeStrings = Enum.GetNames(typeof(FieldType));

        public FieldTypeAttribute(FieldType fieldType)
        {
            FieldType = fieldType;

            // Set some DisplayFormat for a few specific data types
            switch (fieldType)
            {
                case FieldType.Date:
                    DisplayFormat = new DisplayFormatAttribute();
                    DisplayFormat.DataFormatString = "{0:d}";
                    DisplayFormat.ApplyFormatInEditMode = true;
                    break;
                case FieldType.Time:
                    DisplayFormat = new DisplayFormatAttribute();
                    DisplayFormat.DataFormatString = "{0:t}";
                    DisplayFormat.ApplyFormatInEditMode = true;
                    break;
                case FieldType.Currency:
                    DisplayFormat = new DisplayFormatAttribute();
                    DisplayFormat.DataFormatString = "{0:C}";

                    // Don't set ApplyFormatInEditMode for currencies because the currency
                    // symbol can't be parsed
                    break;
            }
        }

        // <summary>
        ///     Constructor that accepts the string name of a custom data type
        /// </summary>
        /// <param name="customDataType">The string name of the custom data type.</param>
        public FieldTypeAttribute(string customFieldType)
            : this(FieldType.Custom)
        {
            CustomFieldType = customFieldType;
        }

        /// <summary>
        ///     Gets the DataType. If it equals DataType.Custom, <see cref="CustomFieldType" /> should also be retrieved.
        /// </summary>
        public FieldType FieldType { get; }

        /// <summary>
        ///     Gets the string representing a custom data type. Returns a non-null value only if <see cref="FieldType" /> is
        ///     DataType.Custom.
        /// </summary>
        public string CustomFieldType { get; }

        /// <summary>
        ///     Gets the default display format that gets used along with this DataType.
        /// </summary>
        public DisplayFormatAttribute DisplayFormat { get; protected set; }

        /// <summary>
        ///     Return the name of the data type, either using the <see cref="FieldType" /> enum or <see cref="CustomFieldType" />
        ///     string
        /// </summary>
        /// <returns>The name of the data type enum</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public virtual string GetDataTypeName()
        {
            EnsureValidDataType();

            if (FieldType == FieldType.Custom)
            {
                // If it's a custom type string, use it as the template name
                return CustomFieldType;
            }
            // If it's an enum, turn it into a string
            // Use the cached array with enum string values instead of ToString() as the latter is too slow
            return _fieldTypeStrings[(int)FieldType];
        }

        /// <summary>
        ///     Override of <see cref="ValidationAttribute.IsValid(object)" />
        /// </summary>
        /// <remarks>This override always returns <c>true</c>.  Subclasses should override this to provide the correct result.</remarks>
        /// <param name="value">The value to validate</param>
        /// <returns>Unconditionally returns <c>true</c></returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override bool IsValid(object value)
        {
            EnsureValidDataType();

            return true;
        }

        /// <summary>
        ///     Throws an exception if this attribute is not correctly formed
        /// </summary>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        private void EnsureValidDataType()
        {
            if (FieldType == FieldType.Custom && string.IsNullOrWhiteSpace(CustomFieldType))
            {
                throw new InvalidOperationException("The custom DataType string cannot be null or empty.");
            }
        }

    }
}
