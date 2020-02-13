using System.Collections.Generic;
using System.Linq;

namespace Deviser.Admin.Config
{
    /// <summary>
    /// Represents the result of an admin validate operation.
    /// </summary>
    public class ValidationResult
    {
        private static readonly ValidationResult _success = new ValidationResult { Succeeded = true };
        private List<ValidationError> _errors = new List<ValidationError>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="Deviser.Admin.Config.ValidationError"/>s containing an errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="Deviser.Admin.Config.ValidationError"/>s.</value>
        public IEnumerable<ValidationError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="ValidationResult"/> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="ValidationResult"/> indicating a successful operation.</returns>
        public static ValidationResult Success => _success;

        /// <summary>
        /// Creates an <see cref="ValidationResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="Deviser.Admin.Config.ValidationError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="ValidationResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static ValidationResult Failed(params ValidationError[] errors)
        {
            var result = new ValidationResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="ValidationResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="ValidationResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned 
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }
}
