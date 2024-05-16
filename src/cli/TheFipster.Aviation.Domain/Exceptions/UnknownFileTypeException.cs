namespace TheFipster.Aviation.Domain.Exceptions
{
    public class UnknownFileTypeException : Exception
    {
        public UnknownFileTypeException() : base() { }
        public UnknownFileTypeException(string message) : base(message) { }
        public UnknownFileTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
