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
    /// <summary>
    /// Implements a Wrapperclass for an DirectRequest. 
    /// An instance of this gets created in the DirectProvider for each RPC in a Request.
    /// 
    /// It contains all needed Information for a method Call
    /// It also contains the HttpContext it was called in.
    /// </summary>
    [JsonObject]
    public class DirectRequest
    {
        /// <summary>
        /// Static Method that determiniate if the given HttpRequest contains a Ext-Direct send by a Form or not
        /// </summary>
        /// <param name="hr">The HttpRequest to check for FormAction Type</param>
        /// <returns>True if the HttpRequest contains a ExtAction sent by a form</returns>
        public static bool isFormAction(HttpRequest hr)
        {
            return (!String.IsNullOrEmpty(hr[DirectRequest.RequestFormAction]));
        }

        // constants for fix Parameter used to define the Direct request during a Form request
        private static string RequestFormTransactionId = "extTID";
        private static string RequestFormAction = "extAction";
        private static string RequestFormMethod = "extMethod";
        private static string RequestFormUpload = "extUpload";
        private static string RequestFormType = "extType";

        /// <summary>
        /// Creates an Instance of this Object from a httpRequest. 
        /// This Constructer is used by the DirectProcessor for Direct Actions Send Fia a Form Submit
        /// </summary>
        /// <param name="httpRequest">The HttpRequest containing the Extaction as Parameters</param>
        internal DirectRequest(HttpContext httpContext) 
        {
            HttpRequest httpRequest = httpContext.Request;
            this.Action = httpRequest[DirectRequest.RequestFormAction] ?? string.Empty;
            this.Method = httpRequest[DirectRequest.RequestFormMethod] ?? string.Empty;
            this.Type = httpRequest[DirectRequest.RequestFormType] ?? string.Empty;
            this.IsUpload = Convert.ToBoolean(httpRequest[DirectRequest.RequestFormUpload]);
            this.IsForm = true;
            this.TransactionId = Convert.ToInt32(httpRequest[DirectRequest.RequestFormTransactionId]);
            this.Data = null;
            this.HttpContext = httpContext;

        }
        
        /// <summary>
        /// Creates an Instance of this Object from a JObject that contains all needed Informations.
        /// This Constructer is used by the DirectProcessor for normal Ext-Direct Actions 
        /// (Action Definitions JSON is send via the request body)
        /// </summary>
        /// <param name="actionObject"></param>
        internal DirectRequest(HttpContext httpContext, JObject actionObject)
        {
            this.Action = ((string)actionObject["action"]);
            this.Method = ((string)actionObject["method"]);
            this.IsForm = false;
            this.TransactionId = ((int)actionObject["tid"]);
            
            this.Type = ((string)actionObject["type"]);
            this.Data = actionObject["data"];
            this.HttpContext = httpContext;
        }
        /// <summary>
        /// The Name of the Action (Class) the Method Ext-Direct wants to call is implemented in
        /// </summary>
        [JsonProperty("action")]
        public string Action
        {
            get;
            private set;
        }

        /// <summary>
        /// The Name of the Methode Ext-Direct wants to call
        /// </summary>
        [JsonProperty("method")]
        public string Method
        {
            get;
            private set;
        }

        /// <summary>
        /// The HttpRequest this DirectRequest was part of
        /// </summary>
        public HttpContext HttpContext
        {
            get;
            private set;
        }

        
        [JsonProperty("data")]
        public Object Data
        {
            get;
            private set;
        }

        [JsonProperty("type")]
        public string Type
        {
            get;
            private set;
        }

        public bool IsForm
        {
            get;
            private set;
        }

        public bool IsUpload
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "tid")]
        public int TransactionId
        {
            get;
            private set;
        }

    }

}
