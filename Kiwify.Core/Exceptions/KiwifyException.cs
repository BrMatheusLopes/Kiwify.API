namespace Kiwify.Core.Exceptions
{
    public class KiwifyException : Exception
    {
        public KiwifyException()
        {
        }

        public KiwifyException(string? message) : base(message)
        {
        }
    }
}
