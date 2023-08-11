using System.Collections.Generic;
using System.Linq;
using ArgumentStringNS;

namespace ArgumentStringNS.Exceptions
{
    public class MissingArgumentException : ParseException
    {
        public MissingArgumentException(IEnumerable<string> missingArguments, ParseOptions options) :
            base(GenerateErrorText(missingArguments, options))
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
