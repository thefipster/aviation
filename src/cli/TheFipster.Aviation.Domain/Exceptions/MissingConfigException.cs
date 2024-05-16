namespace TheFipster.Aviation.Domain.Exceptions
{
    public class MissingConfigException : Exception
    {
        public MissingConfigException() : base() { }
        public MissingConfigException(string message) : base(message) { }
        public MissingConfigException(string message, Exception inner) : base(message, inner) { }
    }
}
