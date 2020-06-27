namespace ContactsBook.Common.Exceptions
{
    public class InvalidParametersException : DomainException
    {
        public InvalidParametersException(string message)
            : base(message)
        { }
    }
}
