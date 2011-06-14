   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using ExtDirect4DotNet.customJsonConverter;
using Newtonsoft.Json.Converters;
using ExtDirect4DotNet.exceptions;

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
                JsonConverterCollection jc = new JsonConverterCollection();

                this.Result = JsonConvert.SerializeObject(result, new JavaScriptDateTimeConverter(), new DataRowConverter(),new DataRowViewConverter(), new DataRowCollectionConverter());
            }


            this.IsUpload = request.IsUpload;
        }

        public DirectResponse(DirectEvent eventObj)
        {
            this.Type = "event";
            this.Name = eventObj.name;
            JsonConverterCollection jc = new JsonConverterCollection();

            this.Result = JsonConvert.SerializeObject(eventObj.data, new JavaScriptDateTimeConverter(), new DataRowConverter(), new DataRowViewConverter(), new DataRowCollectionConverter());
        }

        public DirectResponse(DirectRequest request, DirectException e)
        {
            

            this.Type = e.type;
            this.TransactionId = request.TransactionId;
            this.Action = request.Action;
            this.Method = request.Method;
            this.Message = e.Message;
            this.Where = e.StackTrace;
            this.Result = JsonConvert.SerializeObject(e.result); ;
            if (e is DirectException)
            {
                this.ErrorCode = ((DirectException)e).errorCode;
                this.ExceptionType = ((DirectException)e).ExcetionType;
                if (((DirectException)e).result != null)
                    this.Result = JsonConvert.SerializeObject(((DirectException)e).result);
            }


            this.IsUpload = request.IsUpload;
        }

        public DirectResponse(DirectRequest request, Exception e)
        {
            if (e is DirectException)
            {
                DirectException de = (DirectException)e;
                this.Type = de.type;
                if (e is DirectParameterException)
                {
                    DirectException de2 = (DirectParameterException)e;
                    if (de2.directRequest == null)
                    {
                        de2.directRequest = request;
                    }
                }
                
            }
            else
            {
                this.Type = "exception";
            }
            this.TransactionId = request.TransactionId;
            this.Action = request.Action;
            this.Method = request.Method;
            this.Message = e.Message;
            this.Where = e.StackTrace;
            this.Result = "{success:false}";
            if (e is DirectException)
            {
                this.ErrorCode = ((DirectException)e).errorCode;
                this.ExceptionType = ((DirectException)e).ExcetionType;
                if (((DirectException)e).result != null)
                    this.Result = JsonConvert.SerializeObject(((DirectException)e).result);
            }

            
            

            

            this.IsUpload = request.IsUpload;
        }

        [JsonProperty(PropertyName = "exceptionType")]
        public string ExceptionType
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "type")]
        public string Type
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
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

        [JsonProperty(PropertyName = "errorcode")]
        public uint ErrorCode
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

            writer.WritePropertyName("name");
            writer.WriteValue(this.Name);

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
            else if (this.Result is string)
            {
                writer.WriteRawValue((string)this.Result);
            }
            else
            {
                writer.WriteValue(this.Result);
            }


            writer.WritePropertyName("message");
            writer.WriteValue(this.Message);


            writer.WritePropertyName("where");
            writer.WriteValue(this.Where);

            writer.WritePropertyName("errorcode");
            writer.WriteValue(this.ErrorCode);

            writer.WritePropertyName("exceptionType");
            writer.WriteValue(this.ExceptionType);
           

            // }
            writer.WriteEndObject();

            
            string json = sw.ToString();
            sw.Close();
            return json;
            //json.Serialize(writer, value);
        }
        
    }
}
