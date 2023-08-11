namespace ArgumentStringNS.Exceptions
{
    public class MalformedArgumentException : ParseException
    {
        public MalformedArgumentException(string item) : base($"Segment `{item}` is malformed.")
        {
        }
    }
}
