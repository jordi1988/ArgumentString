using System;
using System.Runtime.Serialization;

namespace ArgumentStringNS.Exceptions
{
    /// <summary>
    /// Exception that is used if a segment cannot be read because no assignment was found.
    /// </summary>
    /// <seealso cref="ParseException" />
    [Serializable]
    public class MalformedArgumentException : ParseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedArgumentException"/> class.
        /// </summary>
        public MalformedArgumentException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedArgumentException"/> class.
        /// </summary>
        /// <param name="segment">The affected segment.</param>
        public MalformedArgumentException(string? segment)
            : base($"Segment `{segment}` is malformed.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedArgumentException"/> class.
        /// </summary>
        /// <param name="segment">The affected segment.</param>
        /// <param name="innerException">The inner exception.</param>
        public MalformedArgumentException(string? segment, Exception innerException)
            : base($"Segment `{segment}` is malformed.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedArgumentException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected MalformedArgumentException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
