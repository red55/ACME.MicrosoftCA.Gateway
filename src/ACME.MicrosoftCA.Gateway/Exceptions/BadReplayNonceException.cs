using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Exceptions
{
    [Serializable]
    public class BadReplayNonceException : ApiException
    {
        private void Init()
        {
            HttpStatus = System.Net.HttpStatusCode.BadRequest;
        }
        public BadReplayNonceException() => Init();

        public BadReplayNonceException(string message) : base(message) => Init();

        public BadReplayNonceException(string message, Exception innerException) : base(message, innerException) => Init();

        protected BadReplayNonceException(SerializationInfo info, StreamingContext context) : base(info, context) => Init();
    }
}
