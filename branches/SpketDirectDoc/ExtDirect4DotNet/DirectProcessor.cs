using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace ExtDirect4DotNet
{
    /// <summary>
    /// Class for processing Ext Direct requests.
    /// </summary>
    public class DirectProcessor
    {

        /// <summary>
        /// Processes an incoming request.
        /// </summary>
        /// <param name="provider">The provider that triggered the request.</param>
        /// <param name="httpRequest">The http information.</param>
        /// <returns>The result from the client method.</returns>
        public static DirectExecution Execute(DirectProvider provider, HttpContext httpContext)
        {
            HttpRequest httpRequest = httpContext.Request;
            // parse the a list of requests from the httpRequest
            List<DirectRequest> requests = ParseRequest(httpRequest);
            List<DirectResponse> responses = new List<DirectResponse>();
          
            // after parsing was successfull Proccess the list of requests
            foreach(DirectRequest request in requests)
            {
                // try to find the method in the provider 
                DirectMethod directMethod = provider.GetDirectMethod(request);
                DirectResponse response;
                

                // try to Invoke the Method and serialize the Data
                try
                {
                    Object result = directMethod.invoke(request, httpContext);

                    
                    // Handle as Poll?
                    if (directMethod.OutputHandling == OutputHandling.Poll)
                    {
                        
                        response = new DirectResponse(request, "{success:true}", directMethod.OutputHandling);
                        responses.Add(response);

                        if (result is List<DirectEvent>)
                        {
                            foreach (DirectEvent currEvent in (List<DirectEvent>)result)
                            {
                                response = new DirectResponse(currEvent);
                                responses.Add(response);
                            }
                        }
                        else
                        {
                            throw new Exception("A Method with outputhandling Poll has to return a List<DirectEvent>");
                        }

                    }
                    else
                    {
                        response = new DirectResponse(request, result, directMethod.OutputHandling);
                        responses.Add(response);

                    }
                }
                catch (TargetInvocationException e)
                {   
                    response = new DirectResponse(request, e.InnerException);
                    responses.Add(response);
                }
                
                

            }

            string output = "[";

            int i3 = 1; 
            foreach(DirectResponse response in responses)
            {
                if(response.Type ==  "exception")
                {
	                directExecution.containsErrors = true;
	            }
                output += response.toJson();
                if (responses.Count > i3) 
                {
                    output += ",";
                }
                i3++;
            }

            output += "]";
            directExecution.jsonResponse = output;
             	
            return directExecution;
        }

        internal static List<DirectRequest> ParseRequest(HttpRequest httpRequest)
        {
            // TODO Throw parse Exception

            List<DirectRequest> proccessList = new List<DirectRequest>();
            if (!String.IsNullOrEmpty(httpRequest[DirectRequest.RequestFormAction]))
            {
                DirectRequest request = new DirectRequest();
                request.Action = httpRequest[DirectRequest.RequestFormAction] ?? string.Empty;
                request.Method = httpRequest[DirectRequest.RequestFormMethod] ?? string.Empty;
                request.Type = httpRequest[DirectRequest.RequestFormType] ?? string.Empty;
                request.IsUpload = Convert.ToBoolean(httpRequest[DirectRequest.RequestFormUpload]);
                request.TransactionId = Convert.ToInt32(httpRequest[DirectRequest.RequestFormTransactionId]);
                request.Data = null;
                request.HttpRequest = httpRequest;
                proccessList.Add(request);
            }
            else
            {
                UTF8Encoding encoding = new UTF8Encoding();
                string json = encoding.GetString(httpRequest.BinaryRead(httpRequest.TotalBytes));


                JArray directRequests;
                try
                {
                    directRequests = JArray.Parse(json);
                }
                catch (Exception e)
                {
                    directRequests = JArray.Parse("["+json+"]");
                }
                          
                foreach (JObject dreq in directRequests)
                {
                    proccessList.Add(new DirectRequest(dreq));
                }
               
            }
            return proccessList;
        }
       

    }
}
