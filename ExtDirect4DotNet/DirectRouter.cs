using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace ExtDirect4DotNet
{
    /// <summary>
    /// represents the HttpHandler Class that handles the all requests done by Ext.Direct's providers
    /// </summary>
    public class DirectRouter : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            
            
            // set default contenttype to json
            context.Response.ContentType = "application/json";

            string responseWrapStart = "";
            string responseWrapEnd = "";

            // check if the request contains a Fileupload. If so the router needs to return an HTML Document containing a Textarea
            if (context.Request.Files.Count > 0)
            {
                // The server response is parsed by the browser to create the document for the IFRAME. 
                // If the server is using JSON to send the return object, then the Content-Type header must 
                // be set to "text/html" in order to tell the browser to insert the text unchanged into the document body.
                context.Response.ContentType = "text/html";
                responseWrapStart = "<html><textarea>";
                responseWrapEnd = "</textarea></html>";
            }

            // execute the Action
           // var rpc = new ExtRPC();


            DirectProvider provider = DirectProxy.getDirectProviderCache("Ext.app.REMOTING_API");
            DirectExceution directExecution = DirectProcessor.Execute(provider, context);

            if (directExecution.containsErrors)
            {
                context.Response.StatusCode = 207;
            }

            // send eventually wraped content back to the browser

            context.Response.Write(responseWrapStart);

            context.Response.Write(directExecution.jsonResponse);


            context.Response.Write(responseWrapEnd);

            context.Response.End();

            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
