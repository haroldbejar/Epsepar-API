using System;

namespace NewEpsepar.Domain.Exceptions
{
    public class DominioException : Exception
    {
        public DominioException() { }
        public DominioException(string message) : base(message) { }
        public DominioException(string message, Exception inner) : base(message, inner) { }
    }

    public class ValidacionException : Exception
    {
        public ValidacionException() { }
        public ValidacionException(string message) : base(message) { }
        public ValidacionException(string message, Exception inner) : base(message, inner) { }
    }
}
