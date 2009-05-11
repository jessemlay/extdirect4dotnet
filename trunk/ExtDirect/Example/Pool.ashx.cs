using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ExtDirect;
using ExtDirect.Direct;


namespace Direct
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Pool : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var res = "Successfully polled at: " + DateTime.Now.ToString();
            var pool = new ExtPool();
            context.Response.Write(pool.BindPool("rpc", "message", res));                        
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
