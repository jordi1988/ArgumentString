using ArgumentStringNS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgumentStringNS
{
    /// <summary>
    /// Main class and entry point for the <c>ArgumentString</c> library.
    /// </summary>
    public class ArgumentString
    {
        private readonly ParseOptions _options;
        private readonly Dictionary<string, string> _arguments = new Dictionary<string, string>();

        /// <summary>
        /// Gets the count of arguments.
        /// </summary>
        public int Count => _arguments.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentString"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="options">The options.</param>
        /// <inheritdoc cref="GenerateArgumentCollection(string)"/>
        public ArgumentString(string arguments, ParseOptions? options = null)
        {
            _options = options ?? new ParseOptions();

            GenerateArgumentCollection(arguments);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentString"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="options">The options.</param>
        public ArgumentString(string arguments, Action<ParseOptions> options) :
                    this(arguments, GetOptions(options))
        {
        }

        /// <summary>
        /// Gets the value for the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Empty string if not found and <see cref="ParseOptions.ThrowOnAccessIfKeyNotFound"/> equals to false.</returns>
        /// <exception cref="MissingArgumentException">Thrown if key is not found and <see cref="ParseOptions.ThrowOnAccessIfKeyNotFound"/> equals to true.</exception>
        /// <remarks>Complexity of this method is <c>O(1)</c> (constant).</remarks>
        public string? Get(string key)
        {
            bool shouldThrowIfKeyMissing = _options.ThrowOnAccessIfKeyNotFound && !_arguments.ContainsKey(key);
            if (shouldThrowIfKeyMissing)
            {
                throw new MissingArgumentException(new string[] { key }, _options);
            }
            
            var value = _arguments.GetValueOrDefault(key);

            bool shouldReturnEmptyString = _options.ReturnEmptyStringInsteadOfNull && value is null;
            if (shouldReturnEmptyString)
            {
                return string.Empty;
            }

            return value;
        }

        /// <summary>
        /// Gets the value for the given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <exception cref="MissingArgumentException">Thrown if index is out of range.</exception>
        /// <remarks>Complexity of this method is <c>O(n)</c> (linear), if <typeparamref name="T"/> is a primitve type.</remarks>
        public string? Get(int index)
        {
            if (index >= _arguments.Count)
            {
                if (_options.ThrowOnAccessIfKeyNotFound)
                {
                    throw new MissingArgumentException(new string[] { $"Index {index}" }, _options);
                }

                if (_options.ReturnEmptyStringInsteadOfNull)
                {
                    return string.Empty;
                }

                return default;
            }

            var keyValuePair = _arguments.ToArray()[index];

            return keyValuePair.Value;
        }

        /// <summary>
        /// Gets the converted value for the given key.
        /// </summary>
        /// <returns>Empty string if not found and <see cref="ParseOptions.ThrowOnAccessIfKeyNotFound"/> equals to false.</returns>
        /// <typeparam name="T">The return type.</typeparam>
        /// <remarks>Complexity of this method is <c>O(1)</c> (constant), if <typeparamref name="T"/> is a primitve type.</remarks>
        /// <inheritdoc cref="ConvertValue{T}(string, string)"/>
        /// <inheritdoc cref="Get(string)"/>
        public T Get<T>(string key)
        {
            var value = Get(key);
            var convertedValue = ConvertValue<T>(key, value);

            return convertedValue;
        }
        /// <summary>
        /// Gets the converted value for the given key.
        /// </summary>
        /// <returns>Empty string if not found and <see cref="ParseOptions.ThrowOnAccessIfKeyNotFound"/> equals to false.</returns>
        /// <typeparam name="T">The return type.</typeparam>
        /// <remarks>Complexity of this method is <c>O(n)</c> (linear), if <typeparamref name="T"/> is a primitve type.</remarks>
        /// <inheritdoc cref="Get(int)"/>
        /// <inheritdoc cref="ConvertValue{T}(string, string)"/>
        public T Get<T>(int index)
        {
            var value = Get(index);
            var convertedValue = ConvertValue<T>($"Index {index}", value);

            return convertedValue;
        }

        /// <inheritdoc cref="Get(string)"/>
        public string? this[string key] => Get(key);

        /// <inheritdoc cref="Get(int)"/>
        public string? this[int index] => Get(index);

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="argumentString">The argument string.</param>
        /// <exception cref="ArgumentException">Argument must not be empty.</exception>
        private static void ValidateInput(string argumentString)
        {
            if (string.IsNullOrWhiteSpace(argumentString))
            {
                throw new ArgumentException("Argument must not be empty.", nameof(argumentString));
            }
        }

        /// <summary>
        /// Converts the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ConversionException"></exception>
        private static T ConvertValue<T>(string key, string? value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new ConversionException(key, ex);
            }
        }

        /// <summary>
        /// Create options based on action type.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private static ParseOptions? GetOptions(Action<ParseOptions>? options)
        {
            var output = new ParseOptions();
            options?.Invoke(output);

            return output;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="argumentString"></param>
        /// <exception cref="ArgumentException">if input is null or duplicate key in input</exception>
        /// <exception cref="MissingArgumentException">if mandatory key is not provided</exception>
        /// <exception cref="MalformedArgumentException">if input has malformed key value pairs</exception>
        /// <returns></returns>
        private void GenerateArgumentCollection(string argumentString)
        {
            ValidateInput(argumentString);

            var missingArguments = GetMandatoryKeys();
            var keyValuePairs = argumentString.Split(_options.ArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in keyValuePairs)
            {
                var pair = GetKeyValuePair(item);

                _arguments.Add(pair.Key, pair.Value);

                missingArguments = missingArguments.Where(x => x != pair.Key);
            }

            ValidateMandatoryArguments(missingArguments);
        }

        /// <summary>
        /// Gets the mandatory keys from the provided options.
        /// </summary>
        /// <returns>Mandatory keys.</returns>
        private IEnumerable<string> GetMandatoryKeys()
        {
            _options!.MandatoryKeys ??= new List<string>();
            var mandatoyKeys = _options.MandatoryKeys.AsEnumerable();

            return mandatoyKeys;
        }

        /// <summary>
        /// Throws <see cref="MissingArgumentException"/> if any mandatory argument is not provided.
        /// </summary>
        /// <param name="missingArguments">The missing mandatory arguments.</param>
        /// <exception cref="MissingArgumentException"></exception>
        private void ValidateMandatoryArguments(IEnumerable<string> missingArguments)
        {
            if (missingArguments.Any())
            {
                throw new MissingArgumentException(missingArguments, _options);
            }
        }

        /// <summary>
        /// Gets the key and the value from a given segment devided by the separator.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>Tuple of key and value.</returns>
        /// <exception cref="MalformedArgumentException"></exception>
        private (string Key, string Value) GetKeyValuePair(string segment)
        {
            var keyValuePair = segment.Split(_options!.KeyValueSeparator, StringSplitOptions.None);
            if (keyValuePair?.Length != 2)
            {
                throw new MalformedArgumentException(segment);
            }

            var key = keyValuePair[0];
            var value = keyValuePair[1];

            return (key, value);
        }
    }
}
