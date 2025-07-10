
namespace _011Global.Shared.Exceptions
{    public class UpdateDBException : Exception
    {
        public UpdateDBException(string message, Exception innerException) : base(message, innerException) { }
        public UpdateDBException(string message) : base(message) { }
    }
}
