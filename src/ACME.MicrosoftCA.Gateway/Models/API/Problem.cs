using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACME.MicrosoftCA.Gateway.Models.API
{
    public enum ProblemType
    {
        none,
        badCSR,                           //The CSR is unacceptable (e.g., due to a short key)
        badNonce,                         //The client sent an unacceptable anti-replay nonce
        badSignatureAlgorithm,            //The JWS was signed with an algorithm the server does not support
        invalidContact,                   //A contact URL for an account was invalid
        unsupportedContact,               //A contact URL for an account used an unsupported protocol scheme
        externalAccountRequired,          //The request must include a value for the “externalAccountBinding” field
        accountDoesNotExist,              //The request specified an account that does not exist
        malformed,                        //The request message was malformed
        rateLimited,                      //The request exceeds a rate limit
        rejectedIdentifier,               //The server will not issue for the identifier
        serverInternal,                   //The server experienced an internal error
        unauthorized,                     //The client lacks sufficient authorization
        unsupportedIdentifier,            //Identifier is not supported, but may be in future
        userActionRequired,               //Visit the “instance” URL and take actions specified there
        badRevocationReason,              //The revocation reason provided is not allowed by the server
        caa,                              //Certification Authority Authorization(CAA) records forbid the CA from issuing
        dns,                              //There was a problem with a DNS query
        connection,                       //The server could not connect to validation target
        tls,                              //The server received a TLS error during validation
        incorrectResponse                 //Response received didn’t match the challenge’s requirements
    }


    [Serializable]
    public class Problem
    {
        public static readonly string ACME_NAMESPACE = @"urn:ietf:params:acme:error:";

        [NonSerialized]
        protected List<Problem> _SubProblems = new List<Problem>();

        [JsonProperty(@"type")]
        [JsonConverter(typeof(Serialization.ProblemTypeJsonConverter))]
        public ProblemType ProblemType { get; set; }

        [JsonProperty(@"detail")]
        public string Detail { get; set; }

    }

    public class ProblemWithSubProblems : Problem
    {
        [JsonProperty(@"subproblems")]
        public Problem [] SubProblems { get => _SubProblems.ToArray(); }

        public void AddSubProblem<T>(T prb) where T : Problem
        {
            if (null == prb)
            {
                throw new ArgumentNullException(@"prb");
            }

            _SubProblems.Add(prb);
        }

        public bool ShouldSerializeSubProblems() =>
            (null != SubProblems) && SubProblems.Any();
    }

    [Serializable]
    public class Identifier
    {
        [JsonProperty(@"type")]
        public string IdentifierType { get; set; }

        [JsonProperty(@"value")]
        public string Value { get; set; }
    }

    [Serializable]
    public class IdentifiedProblem : Problem
    {

        [JsonProperty(@"identifier")]
        public Identifier ProblemIdentifier { get; set; }

        public bool ShouldSerializeIdentifier() =>
            ProblemIdentifier != null;
    }
}
