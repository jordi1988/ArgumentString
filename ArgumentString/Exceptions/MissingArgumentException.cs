using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ArgumentStringNS.Exceptions
{
    /// <summary>
    /// Exception that is used if an argument is missing, either because of mandatory arguments or because of an argument that does not exsist when accessed.
    /// </summary>
    /// <seealso cref="ArgumentStringNS.Exceptions.ParseException" />
    [Serializable]
    public class MissingArgumentException : ParseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingArgumentException"/> class.
        /// </summary>
        public MissingArgumentException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingArgumentException"/> class.
        /// </summary>
        /// <param name="missingArguments">The missing arguments.</param>
        /// <param name="options">The options.</param>
        public MissingArgumentException(IEnumerable<string> missingArguments, ParseOptions options)
            : base(GenerateErrorText(missingArguments, options))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingArgumentException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MissingArgumentException(string? message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingArgumentException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected MissingArgumentException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        private static string? GenerateErrorText(IEnumerable<string> missingArguments, ParseOptions options)
        {
            var mandatory = !missingArguments.Except(options.MandatoryKeys).Any() ? "mandatory " : string.Empty;
            var keysAre = missingArguments.Count() == 1 ? "key is" : "keys are";
            var missingArgumentsList = string.Join(", ", missingArguments);

            return $"The {mandatory}{keysAre} missing: {missingArgumentsList}.";
        }
    }
}
