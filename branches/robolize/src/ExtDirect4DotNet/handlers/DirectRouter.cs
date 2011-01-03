using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using log4net;
using Newtonsoft.Json.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Implementation of <see cref="IHttpHandler"/> responsible for handling Ext.Direct requests.
    /// </summary>
    public class DirectRouter : IHttpHandler, IRequiresSessionState {
        private static readonly ILog Logger = LogManager.GetLogger("ExtDirect4DotNet.JSON");

        /// <summary>
        /// Gets the current instance of the <see cref="HttpContext"/> passed into this handler.
        /// </summary>
        /// <value>The HTTP context.</value>
        public HttpContext HttpContext { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context) {
            HttpContext = context;

            // set default ContentType to json
            HttpContext.Response.ContentType = "application/json";

            string responseWrapStart = "";
            string responseWrapEnd = "";

            // check if the request contains a file upload. If so the router needs to return an HTML Document containing a text area
            if (HttpContext.Request.Files.Count > 0) {
                // The server response is parsed by the browser to create the document for the IFRAME. 
                // If the server is using JSON to send the return object, then the Content-Type header must
                // be set to "text/html" in order to tell the browser to insert the text unchanged into the document body.
                HttpContext.Response.ContentType = "text/html";
                responseWrapStart = "<html><textarea>";
                responseWrapEnd = "</textarea></html>";
            }

            //Execute the DirectAction.
            DirectProvider directProvider = DirectProxy.GetDirectProviderCache();
            List<DirectRequest> requests = ParseRequest();
            DirectProcessor directProcessor = new DirectProcessor();
            directProcessor.DirectMethodError += OnDirectMethodError;
            DirectExecution directExecution = directProcessor.Execute(requests, directProvider);
            if (directExecution.containsErrors) {
                HttpContext.Response.StatusCode = 207;
            }

            string response = directExecution.jsonResponse;
            Logger.Info(string.Format("Response: {0}", response));

            // send eventually wrapped content back to the browser
            HttpContext.Response.Write(responseWrapStart);
            HttpContext.Response.Write(response);
            HttpContext.Response.Write(responseWrapEnd);
            HttpContext.Response.End();
        }

        internal List<DirectRequest> ParseRequest() {
            HttpRequest httpRequest = HttpContext.Current.Request;

            List<DirectRequest> proccessList = new List<DirectRequest>();
            if (!string.IsNullOrEmpty(httpRequest[DirectRequest.RequestFormAction])) {
                DirectRequest request = new DirectRequest {
                    Action = httpRequest[DirectRequest.RequestFormAction] ?? string.Empty,
                    Method = httpRequest[DirectRequest.RequestFormMethod] ?? string.Empty,
                    Type = httpRequest[DirectRequest.RequestFormType] ?? string.Empty,
                    IsUpload = Convert.ToBoolean(httpRequest[DirectRequest.RequestFormUpload]),
                    TransactionId = Convert.ToInt32(httpRequest[DirectRequest.RequestFormTransactionId]),
                    Data = null,
                    IsForm = true
                };
                proccessList.Add(request);
            }
            else {
                UTF8Encoding encoding = new UTF8Encoding();
                string json = encoding.GetString(httpRequest.BinaryRead(httpRequest.TotalBytes));

                JArray jArrayRequests;
                try {
                    jArrayRequests = JArray.Parse(json);
                }
                catch (Exception) {
                    jArrayRequests = JArray.Parse(string.Format("[{0}]", json));
                }

                foreach (JObject oRequest in jArrayRequests) {
                    DirectRequest directRequest = new DirectRequest(oRequest);
                    string requestString = Newtonsoft.Json.JsonConvert.SerializeObject(directRequest);
                    Logger.Info(string.Format("Request: {0}", requestString));
                    proccessList.Add(directRequest);
                }
            }
            return proccessList;
        }

        /// <summary>
        /// Override this method to apply special handling when an unhandled exception occurs within a DirectMethod invocation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ExtDirect4DotNet.DirectMethodErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectMethodError(object sender, DirectMethodErrorEventArgs args) {
            DirectResponse directResponse = args.GetDefaultDirectResponse();
            args.DirectResponse = directResponse;
        }
    }
}