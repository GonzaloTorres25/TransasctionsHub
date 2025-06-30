namespace _011Global.Shared.Exceptions
{
    public class AddDBException : Exception
    {
        public AddDBException(string message, Exception innerException) : base(message, innerException) { }
        public AddDBException(string message) : base(message) { }
    }
}
