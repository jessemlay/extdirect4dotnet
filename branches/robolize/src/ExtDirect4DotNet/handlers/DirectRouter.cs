using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Implementation of <see cref="IHttpHandler"/> responsible for handling Ext.Direct requests.
    /// </summary>
    public class DirectRouter : IHttpHandler, IRequiresSessionState {
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

            // set default contenttype to json
            HttpContext.Response.ContentType = "application/json";

            string responseWrapStart = "";
            string responseWrapEnd = "";

            // check if the request contains a Fileupload. If so the router needs to return an HTML Document containing a Textarea
            if (HttpContext.Request.Files.Count > 0) {
                // The server response is parsed by the browser to create the document for the IFRAME. 
                // If the server is using JSON to send the return object, then the Content-Type header must 
                // be set to "text/html" in order to tell the browser to insert the text unchanged into the document body.
                HttpContext.Response.ContentType = "text/html";
                responseWrapStart = "<html><textarea>";
                responseWrapEnd = "</textarea></html>";
            }

            //Execute the DirectAction.
            DirectExecution directExecution = new DirectProcessor(this).Execute();
            if (directExecution.containsErrors) {
                HttpContext.Response.StatusCode = 207;
            }

            // send eventually wraped content back to the browser
            HttpContext.Response.Write(responseWrapStart);
            HttpContext.Response.Write(directExecution.jsonResponse);
            HttpContext.Response.Write(responseWrapEnd);
            HttpContext.Response.End();
        }

        internal virtual DirectResponse OnError(DirectRequest directRequest, TargetInvocationException exception) {
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