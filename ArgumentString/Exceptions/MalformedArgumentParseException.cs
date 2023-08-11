namespace ArgumentStringNS.Exceptions
{
    public class MalformedArgumentParseException : ParseException
    {
        public MalformedArgumentParseException(string item) : base($"`{item}` is malformed.")
        {
        }
    }
}
