namespace Domain.Exceptions
{
    public class EnumerationException : DomainException
    {
        public EnumerationException()
        { }

        public EnumerationException(object value)
            : base($"{value} is unrecognized value")
        { }
    }
}