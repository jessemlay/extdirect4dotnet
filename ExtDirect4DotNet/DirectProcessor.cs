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
using System.Collections;
using ExtDirect4DotNet.interfaces;
using ExtDirect4DotNet.exceptions;

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
        /// <param name="httpRequest">The httpContext that contains all information about the procedures to call.</param>
        /// <returns>The result from the client method.</returns>
        public static DirectExecution Execute(DirectProvider provider, HttpContext httpContext)
        {

            HttpRequest httpRequest = httpContext.Request;

            // parse the a list of requests from the httpRequest
            List<DirectRequest> requests = ParseRequest(httpContext);

            // create a list of responses gets filled with each response form an execution
            List<DirectResponse> responses = new List<DirectResponse>();
           
            Hashtable instances = new Hashtable();
            // after parsing was successfull Proccess the list of requests
            foreach(DirectRequest directRequest in requests)
            {
                // use the Provider to try to find the cached method in the provider 
                DirectMethod directMethod = provider.GetDirectMethod(directRequest);
                DirectResponse response;

                

                // try to Invoke the Method and deserialize the Data
                try
                {
                    // let the DirectMethod invoke its underlying function und the current httpContext
                    Object result = directMethod.invoke(directRequest, instances);

                    /*
                     * handle the special case of a Poll rpc
                     * 
                     * A Poll RPC is a single ExtDirect transaction which may lead to several 
                     * So called ExtDirect Event.
                     * So a Poll Request Method will return an sucess for its own transaction and 
                     * a List of Events 
                     * 
                     */
                    if (directMethod.OutputHandling == OutputHandling.Poll)
                    {
                        // create a response object for the Poll request and add it to the responses
                        response = new DirectResponse(directRequest, "{success:true}", directMethod.OutputHandling);
                        responses.Add(response);

                        // check if the result is a list of Direct Events (needs to be in case of a Poll Method)
                        if (result is List<DirectEvent>)
                        {
                            // if so add a Direct response - Event for each event  
                            foreach (DirectEvent currEvent in (List<DirectEvent>)result)
                            {
                                response = new DirectResponse(currEvent);
                                responses.Add(response);
                            }
                        }
                        else
                        {
                            // if the method doesn't return a list of DirectEvent's throw an exception
                            // TODO throw a nicer Exception
                            throw new Exception("A Method with outputhandling Poll has to return a List<DirectEvent>");
                        }

                    }
                    else
                    {
                        // In case its a normal direct Method (means not a Poll method) create a Response for it.
                        response = new DirectResponse(directRequest, result, directMethod.OutputHandling);
                        responses.Add(response);
                    }
                }
                catch (TargetInvocationException e)
                {
                    // catch all exceptions may occure cause of the invocation via directMetod.invoke 
                    // and add those exceptions to the responses
                    response = new DirectResponse(directRequest, e.InnerException);
                    responses.Add(response);
                }
                catch (DirectException e2)
                {

                    // catch all exceptions may occure cause of the invocation via directMetod.invoke 
                    // and add those exceptions to the responses
                    response = new DirectResponse(directRequest, e2);
                    responses.Add(response);
                }
                catch (Exception e3)
                {
                    string test = "";
                }
            }

            // call the beforeResponse method on all instances tahat implement IActionWithBeforeResponse
            foreach (string key in instances.Keys)
            {
                Object actionInstanz = instances[key];

                Boolean isIActionWithBeforeDestroy = false;
                // now check if the class should recieve some metaData from the request.
                foreach (var i in actionInstanz.GetType().GetInterfaces())
                    if (i == typeof(IActionWithBeforeDestroy))
                    {
                        isIActionWithBeforeDestroy = true;
                        break;
                    }

                if (isIActionWithBeforeDestroy)
                {
                    ((IActionWithBeforeDestroy)actionInstanz).beforeDestroy(httpContext);
                }
            }

            // create the directexecution Object we want to return
            DirectExecution directExecution = new DirectExecution();

            // create the json String we want to pass into the DirectExecution Objects jsonResponse member.
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

        /// <summary>
        /// Parses the http Request to a List of DirectRequest 
        /// </summary>
        /// <param name="httpContext">the httpContext containing the request wich contains the Ext-Direct Requests</param>
        /// <returns>The list of Direct Actions the httpRequest contained</returns>
        internal static List<DirectRequest> ParseRequest(HttpContext httpContext)
        {
            // TODO Throw parse Exception

            List<DirectRequest> proccessList = new List<DirectRequest>();

            // check if the httprequest contains the parameter "extAction" which indicats the request as a 
            // Ext-Direct Form posting
            if (DirectRequest.isFormAction(httpContext.Request))
            {
                // in the case of  a Form post the request can only contain one execution to process (the actual form)
                // configure the DirectRequest for the need of the actual form post processing
                DirectRequest request = new DirectRequest(httpContext);
                
                proccessList.Add(request);
            }
            else
            {
                // set the encoding to utf8 and read out the request into a String 
                UTF8Encoding encoding = new UTF8Encoding();
                string json = encoding.GetString(httpContext.Request.BinaryRead(httpContext.Request.TotalBytes));


                // in case it is not a Form the request may contain a single remote call
                // or an array of remote call

                JArray directRequests;

                // the check for the curly bracket is not enought than the JArray.Parse fails so catch the error 
                try
                {
                    // first check if its a single remotecall, if so, wrap it into squared brackets and make it an array
                    if (json[0] == '{')
                    {
                        directRequests = JArray.Parse("[" + json + "]");
                    }
                    else
                    {
                        // first check if its a batch of remote cales (the first character will be a [ cause its an javascript array)
                        directRequests = JArray.Parse(json);
                    }
                }
                catch (Exception e)
                {
                    // retry the parse with squared brackets
                    directRequests = JArray.Parse("["+json+"]");
                }
                         
                // now create an instance of DirecetRequest for each item in the JArray
                foreach (JObject dreq in directRequests)
                {
                    proccessList.Add(new DirectRequest(httpContext, dreq));
                }
               
            }
            return proccessList;
        }
       

    }
}
