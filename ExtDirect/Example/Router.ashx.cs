using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ExtDirect;
using ExtDirect.Direct;

namespace ExtDirect.Example
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Router : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            var rpc = new ExtRPC();
            context.Response.Write(rpc.ExecuteRPC(context.Request));
            
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
