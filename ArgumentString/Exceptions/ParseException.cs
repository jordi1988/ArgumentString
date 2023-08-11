using System;

namespace ArgumentStringNS.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException(string? message) : base(message)
        {
        }
    }
}
