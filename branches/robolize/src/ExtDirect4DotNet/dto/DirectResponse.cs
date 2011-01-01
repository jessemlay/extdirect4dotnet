using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExtDirect4DotNet {
    [JsonObject]
    public class DirectResponse {
        public DirectResponse(DirectRequest request, object result, OutputHandling outputHandling) {
            Type = request.Type;
            TransactionId = request.TransactionId;
            Action = request.Action;
            Method = request.Method;

            // TODO maybe there is a better way to avoid reparsing a string to json....
            if (outputHandling == OutputHandling.JSON) {
                Result = result;
            }
            else {
                //FUTURE:Expose API or some kind of configuration to allow users to add additional converters if needed. 
                Result = JsonConvert.SerializeObject(result, new JavaScriptDateTimeConverter());
            }

            IsUpload = request.IsUpload;
        }

        public DirectResponse(DirectEvent directEvent) {
            Type = "event";
            Name = directEvent.name;
            //FUTURE:Expose API or some kind of configuration to allow users to add additional converters if needed. 
            Result = JsonConvert.SerializeObject(directEvent.data, new JavaScriptDateTimeConverter());
        }

        public DirectResponse(DirectRequest request, Exception exception) {
            Type = "exception";
            TransactionId = request.TransactionId;
            Action = request.Action;
            Method = request.Method;
            Message = exception.Message;
            Where = exception.StackTrace;
            /*if (exception is DirectException) {
                ErrorCode = ((DirectException) exception).ErrorCode;
            }*/
            Result = "{success:false}";

            IsUpload = request.IsUpload;
        }

        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        /*[JsonProperty(PropertyName = "errorcode")]
        public uint ErrorCode { get; set; }*/

        [JsonIgnore]
        public bool IsUpload { get; private set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "result")]
        public object Result { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public int TransactionId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "where")]
        public string Where { get; set; }

        public string toJson() {
            StringWriter sw = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(sw);

            writer.Formatting = Formatting.None; // Indented;

            writer.QuoteChar = '"';

            // {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue(Type);

            writer.WritePropertyName("name");
            writer.WriteValue(Name);

            writer.WritePropertyName("tid");
            writer.WriteValue(TransactionId);

            writer.WritePropertyName("action");
            writer.WriteValue(Action);

            writer.WritePropertyName("method");
            writer.WriteValue(Method);

            writer.WritePropertyName("result");

            if (Result == null) {
                writer.WriteRawValue("null");
            }
            else {
                writer.WriteRawValue((string) Result);
            }

            writer.WritePropertyName("message");
            writer.WriteValue(Message);

            writer.WritePropertyName("where");
            writer.WriteValue(Where);

            /*writer.WritePropertyName("errorcode");
            writer.WriteValue(ErrorCode);*/

            // }
            writer.WriteEndObject();

            string json = sw.ToString();
            sw.Close();
            return json;
            //json.Serialize(writer, value);
        }
    }
}