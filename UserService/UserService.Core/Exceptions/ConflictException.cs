using System.Runtime.Serialization;

namespace UserService.Core.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string? message) : base(message)
        {
        }

        public ConflictException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}