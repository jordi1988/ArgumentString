using System;
using System.Collections.Generic;
using System.Linq;
using ArgumentStringNS.Exceptions;

namespace ArgumentStringNS
{
    public class ArgumentString
    {
        private readonly ParseOptions _options;
        private readonly Dictionary<string, string> _arguments = new Dictionary<string, string>();
        public ArgumentString(string arguments, ParseOptions? options = null)
        {
            _options = options ?? new ParseOptions();

            GenerateArgumentCollection(arguments);
        }

        public ArgumentString(string arguments, Action<ParseOptions>? options = null) : 
            this(arguments, GetOptions(options))
        {
        }

        private static ParseOptions? GetOptions(Action<ParseOptions>? options)
        {
            var options1 = new ParseOptions();
            options?.Invoke(options1);

            return options1;
        }

        /// <summary>
        /// Gets the value for the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Empty string if not found and <see cref="ParseOptions.ThrowOnAccessIfKeyNotFound"/> equals to false.</returns>
        /// <exception cref="MissingArgumentsParseException">Thrown if key is not found and <see cref="ParseOptions.ThrowOnAccessIfKeyNotFound"/> equals to true.</exception>
        public string Get(string key)
        {
            if (_options.ThrowOnAccessIfKeyNotFound &&
                !_arguments.ContainsKey(key))
            {
                throw new MissingArgumentsParseException(new string[] { key }, _options);
            }

            return _arguments.GetValueOrDefault(key) ?? string.Empty;
        }

        /// <inheritdoc cref="Get(string)"/>
        public string this[string key] => Get(key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="ArgumentException">if input is null or duplicate key in input</exception>
        /// <exception cref="MissingArgumentsParseException">if mandatory key is not provided</exception>
        /// <exception cref="MalformedArgumentParseException">if input has malformed key value pairs</exception>
        /// <returns></returns>
        private void GenerateArgumentCollection(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Argument must not be empty.", nameof(input));
            }

            _options!.MandatoryKeys ??= new List<string>();
            var missingMandatoryArguments = _options.MandatoryKeys.AsEnumerable();

            var keyValuePairs = input.Split(_options.ArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in keyValuePairs)
            {
                var pair = GetKeyValuePair(item);

                _arguments.Add(pair.Key, pair.Value);

                missingMandatoryArguments = missingMandatoryArguments.Where(x => x != pair.Key);
            }

            if (missingMandatoryArguments.Any())
            {
                throw new MissingArgumentsParseException(missingMandatoryArguments, _options);
            }
        }

        private (string Key, string Value) GetKeyValuePair(string item)
        {
            var keyValuePair = item.Split(_options!.KeyValueSeparator, StringSplitOptions.None);
            if (keyValuePair?.Length != 2)
            {
                throw new MalformedArgumentParseException(item);
            }

            var key = keyValuePair[0];
            var value = keyValuePair[1];

            return (key, value);
        }
    }
}
