using System;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExtDirect4DotNet {
    [JsonObject]
    public class DirectRequest {
        public const string RequestFormAction = "extAction";

        public const string RequestFormMethod = "extMethod";

        public const string RequestFormTransactionId = "extTID";

        public const string RequestFormType = "extType";

        public const string RequestFormUpload = "extUpload";

        internal DirectRequest() {
        }

        internal DirectRequest(JObject actionObject) {
            Action = ((string) actionObject["action"]);
            Method = ((string) actionObject["method"]);

            TransactionId = ((int) actionObject["tid"]);

            Type = ((string) actionObject["type"]);
            Data = actionObject["data"];
        }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public Object Data { get; set; }

        public HttpRequest HttpRequest { get; set; }

        public bool IsForm { get; set; }

        public bool IsUpload { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public int TransactionId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}