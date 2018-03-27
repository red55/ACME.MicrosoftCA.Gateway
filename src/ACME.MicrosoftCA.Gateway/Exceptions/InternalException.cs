using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ACME.MicrosoftCA.Gateway.Exceptions
{
    [Serializable]
    public class InternalException : ApiException
    {

        protected override void Init()
        {
            HttpStatus = System.Net.HttpStatusCode.InternalServerError;
            Problem = new Models.API.Problem
            {
                ProblemType = Models.API.ProblemType.serverInternal,
                Detail = Message
            };
        }
        public InternalException() => Init();

        public InternalException(string message) : base(message) => Init();

        public InternalException(string message, Exception innerException) : base(message, innerException) => Init();

        protected InternalException(SerializationInfo info, StreamingContext context) : base(info, context) => Init();
    }
}
