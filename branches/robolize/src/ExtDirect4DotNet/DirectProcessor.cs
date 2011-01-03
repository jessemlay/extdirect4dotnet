using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExtDirect4DotNet {
    //TODO:Need to do a full review of this class.
    /// <summary>
    /// Class for processing Ext Direct requests.
    /// </summary>
    internal class DirectProcessor {
        /// <summary>
        /// Processes an incoming request.
        /// </summary>
        /// <returns>The result from the client method.</returns>
        internal DirectExecution Execute(List<DirectRequest> requests, DirectProvider directProvider) {
            //Parse the a list of requests from the httpRequest.
            List<DirectResponse> responses = new List<DirectResponse>();

            // after parsing was successful process the list of requests
            foreach (DirectRequest request in requests) {
                // try to find the method in the provider 
                DirectMethod directMethod = directProvider.GetDirectMethod(request.Action, request.Method);

                // try to Invoke the Method and serialize the Data
                try {
                    object result = directMethod.Invoke(request);

                    // Handle as Poll?
                    if (directMethod.OutputHandling == OutputHandling.Poll) {
                        //TODO:Verify the handling of the response.
                        DirectResponse response = new DirectResponse(request, "{success:true}", directMethod.OutputHandling);
                        responses.Add(response); //TODO:Should this "Add" be added here?

                        if (!(result is List<DirectEvent>)) {
                            throw new Exception("A Method with output handling Poll is required to return List<DirectEvent>.");
                        }

                        responses.AddRange(((List<DirectEvent>) result).Select(de => new DirectResponse(de)));
                    }
                    else {
                        responses.Add(new DirectResponse(request, result, directMethod.OutputHandling));
                    }
                }
                catch (TargetInvocationException ex) {
                    DirectResponse response = OnError(request, ex);
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

        private DirectResponse OnError(DirectRequest directRequest, TargetInvocationException exception) {
            DirectMethodErrorEventArgs args = new DirectMethodErrorEventArgs(directRequest, exception);
            EventHandler<DirectMethodErrorEventArgs> handler = DirectMethodError;
            if (handler != null) {
                handler(this, args);
            }

            return args.DirectResponse;
        }

        /// <summary>
        /// Occurs when an unhandled exception is thrown during the invocation of a DirectMethod.
        /// </summary>
        public event EventHandler<DirectMethodErrorEventArgs> DirectMethodError;
    }
}