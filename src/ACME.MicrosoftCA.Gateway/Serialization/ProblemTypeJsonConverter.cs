using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACME.MicrosoftCA.Gateway.Serialization
{
    public class ProblemTypeJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Models.API.ProblemType e)
            {
                var s = Enum.GetName(e.GetType(), e);
                writer.WriteValue(Models.API.Problem.ACME_NAMESPACE + s);
            }
            else
            {
                throw new Newtonsoft.Json.JsonException($"Value is not of type {nameof(Models.API.ProblemType)}: {value.GetType().ToString()}");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Models.API.ProblemType);
        
    }
}
