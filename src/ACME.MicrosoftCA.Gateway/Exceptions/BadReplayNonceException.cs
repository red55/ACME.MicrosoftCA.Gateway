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
        protected string Nonce { get; set; }

        private void Init()
        {
            HttpStatus = System.Net.HttpStatusCode.BadRequest;
            Problem = new Models.API.Problem
            {
                ProblemType = Models.API.ProblemType.badNonce,
                Detail = Message
            };
        }
        public BadReplayNonceException() => Init();

        public BadReplayNonceException(string message) : base(message) => Init();

        public BadReplayNonceException(string message, Exception innerException) : base(message, innerException) => Init();

        protected BadReplayNonceException(SerializationInfo info, StreamingContext context) : base(info, context) => Init();
    }
}
