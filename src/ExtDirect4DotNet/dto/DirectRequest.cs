using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExtDirect4DotNet {
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{Action,nq}.{Method,nq}: Data = {Data}")]
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
        public object Data { get; set; }

        public bool IsForm { get; set; }

        public bool IsUpload { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("tid")]
        public int TransactionId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}