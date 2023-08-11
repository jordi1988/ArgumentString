using System.Collections.Generic;
using System.Linq;

namespace ArgumentStringNS
{
    public class ParseOptions
    {
        public List<string> MandatoryKeys { get; set; } = new List<string>();

        public string ArgumentSeparator { get; set; } = ";";

        public string KeyValueSeparator { get; set; } = "=";

        public bool ThrowOnAccessIfKeyNotFound { get; set; }

        public ParseOptions()
        {
        }

        public ParseOptions(params string[] mandatoryKeys)
        {
            MandatoryKeys = mandatoryKeys.ToList();
        }
    }
}
