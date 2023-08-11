using ArgumentStringNS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgumentStringNS
{
    /*
     TODO: Get<int>() as type a la SapConnector
           Code Generator?
           Big O Notation in readme
     */

    public class ArgumentString
    {
        private readonly ParseOptions _options;
        private readonly Dictionary<string, string> _arguments = new Dictionary<string, string>();

        public ArgumentString(string arguments, ParseOptions? options = null)
        {
            _options = options ?? new ParseOptions();

            GenerateArgumentCollection(arguments);
        }

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
        public string Get(string key)
        {
            if (_options.ThrowOnAccessIfKeyNotFound &&
                !_arguments.ContainsKey(key))
            {
                throw new MissingArgumentException(new string[] { key }, _options);
            }

            return _arguments.GetValueOrDefault(key) ?? string.Empty;
        }

        /// <summary>
        /// Gets the converted value for the given key.
        /// </summary>
        /// <returns>Empty string if not found and <see cref="ParseOptions.ThrowOnAccessIfKeyNotFound"/> equals to false.</returns>
        /// <inheritdoc cref="Get(string)"/>
        /// <inheritdoc cref="Convert.ChangeType(object, Type)"/>
        /// <typeparam name="T">The return type.</typeparam>
        public T Get<T>(string key)
        {
            var value = Get(key);

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Gets the converted value for the given key.
        /// </summary>
        /// <returns>Empty string if not found and <see cref="ParseOptions.ThrowOnAccessIfKeyNotFound"/> equals to false.</returns>
        /// <inheritdoc cref="Get(int)"/>
        /// <inheritdoc cref="Convert.ChangeType(object, Type)"/>
        /// <typeparam name="T">The return type.</typeparam>
        public T Get<T>(int index)
        {
            var value = Get(index);

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Gets the value for the given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <exception cref="MissingArgumentException">Thrown if index is out of range.</exception>
        public string Get(int index)
        {
            if (index >= _arguments.Count)
            {
                if (_options.ThrowOnAccessIfKeyNotFound)
                {
                    throw new MissingArgumentException(new string[] { $"Index {index}" }, _options);
                }

                return string.Empty;
            }

            var keyValuePair = _arguments.ToArray()[index];

            return keyValuePair.Value;
        }

        private static ParseOptions? GetOptions(Action<ParseOptions>? options)
        {
            var options1 = new ParseOptions();
            options?.Invoke(options1);

            return options1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="ArgumentException">if input is null or duplicate key in input</exception>
        /// <exception cref="MissingArgumentException">if mandatory key is not provided</exception>
        /// <exception cref="MalformedArgumentException">if input has malformed key value pairs</exception>
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
                throw new MissingArgumentException(missingMandatoryArguments, _options);
            }
        }

        private (string Key, string Value) GetKeyValuePair(string item)
        {
            var keyValuePair = item.Split(_options!.KeyValueSeparator, StringSplitOptions.None);
            if (keyValuePair?.Length != 2)
            {
                throw new MalformedArgumentException(item);
            }

            var key = keyValuePair[0];
            var value = keyValuePair[1];

            return (key, value);
        }

        /// <inheritdoc cref="Get(string)"/>
        public string this[string key] => Get(key);

        /// <inheritdoc cref="Get(int)"/>
        public string this[int index] => Get(index);
    }
}
