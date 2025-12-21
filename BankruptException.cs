using System;
using System.Runtime.Serialization;

namespace Inheritence
{
    [Serializable]
    public class BankruptException : ApplicationException
    {
        public DateTime Time { get; private set; }

        public BankruptException()
        {
            Time = DateTime.Now;
        }

        public BankruptException(string message) : base(message)
        {
            Time = DateTime.Now;
        }

        public BankruptException(string message, Exception innerException) : base(message, innerException)
        {
            Time = DateTime.Now;
        }

        protected BankruptException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Time = DateTime.Now;
        }
    }
}