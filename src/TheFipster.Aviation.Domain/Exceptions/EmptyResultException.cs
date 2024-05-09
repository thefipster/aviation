namespace TheFipster.Aviation.Domain.Exceptions
{
    public class EmptyResultException : Exception
    {
        public EmptyResultException() : base() { }
        public EmptyResultException(string message) : base(message) { }
        public EmptyResultException(string message, Exception inner) : base(message, inner) { }
    }
}
