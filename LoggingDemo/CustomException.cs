using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LoggingDemo
{
    internal class CustomException : Exception
    {
        public string MyProperty { get; set; } = "xxx";

        public CustomException()
        {
        }

        public CustomException(string? message) : base(message)
        {
        }

        public CustomException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public CustomException(Exception? innerException) : base(null, innerException)
        {
        }

        protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
