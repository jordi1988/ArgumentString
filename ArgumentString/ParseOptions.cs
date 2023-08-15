using System.Collections.Generic;
using System.Linq;

namespace ArgumentStringNS
{
    /// <summary>
    /// This class controls the behavior of the parser.
    /// </summary>
    public class ParseOptions
    {
        /// <summary>
        /// Gets or sets the mandatory keys.
        /// </summary>
        /// <remarks><see cref="Exceptions.MissingArgumentException"/> will be thrown if a mandatory key is not provided.</remarks>
        public List<string> MandatoryKeys { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the argument separator that devides the arguments.
        /// </summary>
        public string ArgumentSeparator { get; set; } = ";";

        /// <summary>
        /// Gets or sets the key value separator that assigns the value to the key.
        /// </summary>
        public string KeyValueSeparator { get; set; } = "=";

        /// <summary>
        /// Throws <see cref="Exceptions.MissingArgumentException" /> if nonexistent key or index is accessed.
        /// </summary>
        /// <remarks>Defaults to <c>false</c>.</remarks>
        public bool ThrowOnAccessIfKeyNotFound { get; set; }

        /// <summary>
        /// Returns <c>string.Empty</c> instead of <c>null</c> on strings if key or index is nonexistent.
        /// </summary>
        /// <remarks>Defaults to <c>true</c>, so you must not deal with <c>null</c> values.</remarks>
        public bool ReturnEmptyStringInsteadOfNull { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseOptions"/> class.
        /// </summary>
        public ParseOptions()
        {
        }

        /// <inheritdoc cref="ParseOptions()"/>
        /// <param name="mandatoryKeys">The mandatory keys.</param>
        public ParseOptions(params string[] mandatoryKeys)
        {
            MandatoryKeys = mandatoryKeys.ToList();
        }
    }
}
