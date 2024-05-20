namespace TheFipster.Aviation.Domain.Exceptions
{
    public class MissingImportException : Exception
    {
        public MissingImportException() : base() { }
        public MissingImportException(string message) : base(message) { }
        public MissingImportException(string message, Exception inner) : base(message, inner) { }
    }
}
