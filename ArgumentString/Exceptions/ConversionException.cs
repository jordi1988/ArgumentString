using ArgumentStringNS.Exceptions;
using System;
using System.Runtime.Serialization;

namespace ArgumentString.Exceptions
{
    /// <summary>
    /// Exception that is used when converting values.
    /// </summary>
    /// <seealso cref="ArgumentStringNS.Exceptions.ParseException" />
    [Serializable]
    public class ConversionException : ParseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionException"/> class.
        /// </summary>
        public ConversionException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionException"/> class.
        /// </summary>
        /// <param name="key">The key whose value thew the exception.</param>
        public ConversionException(string? key) 
            : base($"Conversion of key `{key}` threw an exception.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionException"/> class.
        /// </summary>
        /// <param name="key">The key whose value thew the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConversionException(string? key, Exception innerException)
            : base($"Conversion of key `{key}` threw an exception.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected ConversionException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
