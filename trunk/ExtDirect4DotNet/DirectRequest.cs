using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Collections;

namespace ExtDirect4DotNet
{
    [JsonObject]
    public class DirectRequest
    {

        public const string RequestFormTransactionId = "extTID";
        public const string RequestFormAction = "extAction";
        public const string RequestFormMethod = "extMethod";
        public const string RequestFormUpload = "extUpload";
        public const string RequestFormType = "extType";

        internal DirectRequest() 
        { }
        internal DirectRequest(JObject actionObject)
        {
            this.Action = ((string)actionObject["action"]);
            this.Method = ((string)actionObject["method"]);
           
            this.TransactionId = ((int)actionObject["tid"]);
            
            this.Type = ((string)actionObject["type"]);
            this.Data = actionObject["data"];
        }

        [JsonProperty("action")]
        public string Action
        {
            get;
            set;
        }

        [JsonProperty("method")]
        public string Method
        {
            get;
            set;
        }

        public HttpRequest HttpRequest
        {
            get;
            set;
        }

        
        [JsonProperty("data")]
        public Object Data
        {
            get;
            set;
        }

        [JsonProperty("type")]
        public string Type
        {
            get;
            set;
        }

        public bool IsForm
        {
            get;
            set;
        }

        public bool IsUpload
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

    }

}
