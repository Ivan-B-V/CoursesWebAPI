using System.Runtime.Serialization;

namespace CoursesWebAPI.Application.Exceptions.BadRequestExceptions
{
    [Serializable]
    internal class AuthenticationBadRequestException : Exception
    {
        public AuthenticationBadRequestException()
        {
        }

        public AuthenticationBadRequestException(string? message) : base(message)
        {
        }

        public AuthenticationBadRequestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AuthenticationBadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}