using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public ApiException() {}

        public ApiException(string message) : base(message) { }

        public ApiException(string message, Exception innerException) : base(message, innerException) { }

        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public HttpStatusCode HttpStatus { get; protected set; }
    }
}
