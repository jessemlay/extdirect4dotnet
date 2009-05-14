   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace ExtDirect4DotNet
{
    
    [JsonObject]
    internal class DirectResponse
    {


        public DirectResponse(DirectRequest request, object result, OutputHandling outputHandling)
        {
            this.Type = request.Type;
            this.TransactionId = request.TransactionId;
            this.Action = request.Action;
            this.Method = request.Method;


            // TODO maybe there is a better way to avoi re parsing a string to json....
            if (outputHandling == OutputHandling.JSON)
            {
                this.Result = result;
            }
            else
            {
                this.Result = JsonConvert.SerializeObject(result);
            }


            this.IsUpload = request.IsUpload;
        }

        public DirectResponse(DirectRequest request, Exception e)
        {
            this.Type = "exception";
            this.TransactionId = request.TransactionId;
            this.Action = request.Action;
            this.Method = request.Method;
            this.Message = e.Message;
            this.Where = e.StackTrace;
            this.Result = "{success:false}";

            

            this.IsUpload = request.IsUpload;
        }

        
        [JsonProperty(PropertyName = "type")]
        public string Type
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "tid")]
        public int TransactionId
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "action")]
        public string Action
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "method")]
        public string Method
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "result")]
        public object Result
        {
            get;
            set;
        }
       
        [JsonProperty(PropertyName = "where")]
        public string Where
        {
            get;
            set;
        }

        [JsonProperty(PropertyName="message")]
        public string Message
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool IsUpload
        {
            get;
            private set;
        }

        public string toJson()
        {

            StringWriter sw = new StringWriter();
            Newtonsoft.Json.JsonTextWriter writer = new JsonTextWriter(sw);
           

            writer.Formatting = Formatting.None;// Indented;

            writer.QuoteChar = '"';


            // {
            writer.WriteStartObject();
            
            writer.WritePropertyName("type");
            writer.WriteValue(this.Type);

            writer.WritePropertyName("tid");
            writer.WriteValue(this.TransactionId);

            writer.WritePropertyName("action");
            writer.WriteValue(this.Action);

            writer.WritePropertyName("method");
            writer.WriteValue(this.Method);

            writer.WritePropertyName("result");

            if (this.Result == null)
            {
                writer.WriteRawValue("null");
            }
            else
            {
                writer.WriteRawValue((string)this.Result);
            }


            writer.WritePropertyName("message");
            writer.WriteValue(this.Message);


            writer.WritePropertyName("where");
            writer.WriteValue(this.Where);
           


            // }
            writer.WriteEndObject();

            
            string json = sw.ToString();
            sw.Close();
            return json;
            //json.Serialize(writer, value);
        }
        
    }
}
