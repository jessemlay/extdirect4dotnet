using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace ExtDirect4DotNet {
    //TODO:Need to do a full review of this class.
    /// <summary>
    /// Class for processing Ext Direct requests.
    /// </summary>
    internal class DirectProcessor {
        private readonly DirectRouter _directRouter;

        internal DirectProcessor(DirectRouter directRouter) {
            _directRouter = directRouter;
        }

        /// <summary>
        /// Processes an incoming request.
        /// </summary>
        /// <returns>The result from the client method.</returns>
        internal DirectExecution Execute() {
            //Parse the a list of requests from the httpRequest.
            List<DirectRequest> requests = ParseRequest();
            List<DirectResponse> responses = new List<DirectResponse>();

            // after parsing was successfull Proccess the list of requests
            foreach (DirectRequest request in requests) {
                // try to find the method in the provider 
                DirectMethod directMethod = DirectProxy.GetDirectProviderCache().GetDirectMethod(request);

                // try to Invoke the Method and serialize the Data
                try {
                    Object result = directMethod.Invoke(request, _directRouter.HttpContext);

                    // Handle as Poll?
                    if (directMethod.OutputHandling == OutputHandling.Poll) {
                        //TODO:Verify the handling of the response.
                        DirectResponse response = new DirectResponse(request, "{success:true}", directMethod.OutputHandling);
                        responses.Add(response); //TODO:Should this "Add" be added here?

                        if (!(result is List<DirectEvent>)) {
                            throw new Exception("A Method with outputhandling Poll is required to return List<DirectEvent>.");
                        }

                        responses.AddRange(((List<DirectEvent>) result).Select(de => new DirectResponse(de)));
                    }
                    else {
                        responses.Add(new DirectResponse(request, result, directMethod.OutputHandling));
                    }
                }
                catch (TargetInvocationException ex) {
                    DirectResponse response = _directRouter.OnError(request, ex);
                    responses.Add(response);
                }
            }

            bool containsErrors = false;
            string output = "[";
            int i3 = 1;
            foreach (DirectResponse response in responses) {
                if (response.Type == "exception") {
                    containsErrors = true;
                }
                output += response.toJson();
                if (responses.Count > i3) {
                    output += ",";
                }
                i3++;
            }
            output += "]";

            DirectExecution directExecution = new DirectExecution {
                jsonResponse = output,
                containsErrors = containsErrors
            };
            return directExecution;
        }

        internal List<DirectRequest> ParseRequest() {
            HttpRequest httpRequest = _directRouter.HttpContext.Request;

            List<DirectRequest> proccessList = new List<DirectRequest>();
            if (!String.IsNullOrEmpty(httpRequest[DirectRequest.RequestFormAction])) {
                DirectRequest request = new DirectRequest {
                    Action = httpRequest[DirectRequest.RequestFormAction] ?? string.Empty,
                    Method = httpRequest[DirectRequest.RequestFormMethod] ?? string.Empty,
                    Type = httpRequest[DirectRequest.RequestFormType] ?? string.Empty,
                    IsUpload = Convert.ToBoolean(httpRequest[DirectRequest.RequestFormUpload]),
                    TransactionId = Convert.ToInt32(httpRequest[DirectRequest.RequestFormTransactionId]),
                    Data = null,
                    HttpRequest = httpRequest
                };
                proccessList.Add(request);
            }
            else {
                UTF8Encoding encoding = new UTF8Encoding();
                string json = encoding.GetString(httpRequest.BinaryRead(httpRequest.TotalBytes));

                JArray directRequests;
                try {
                    directRequests = JArray.Parse(json);
                }
                catch (Exception) {
                    directRequests = JArray.Parse("[" + json + "]");
                }

                foreach (JObject dreq in directRequests) {
                    proccessList.Add(new DirectRequest(dreq));
                }
            }
            return proccessList;
        }
    }
}