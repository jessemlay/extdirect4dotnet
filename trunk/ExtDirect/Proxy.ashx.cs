using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ExtDirect;
using ExtDirect.Direct;

namespace ExtDirect
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Proxy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {            
            context.Response.ContentType = "text/JavaScript";
            string rem = "{";
            rem += "url: \"../Router.ashx\",";
            rem += "type:\"remoting\",";

            string json = DirectProxyGenerator.generateDirectApi();
            rem += json;
            rem += "};";
            rem = "Ext.app.REMOTING_API =" + rem;
            rem = "(function(){" + rem + "})();";

            context.Response.Write(rem);
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
