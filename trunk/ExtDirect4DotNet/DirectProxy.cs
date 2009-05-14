using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using ExtDirect4DotNet;
using ExtDirect4DotNet.helper;

namespace ExtDirect4DotNet
{
    /// <summary>
    /// Represents the Proxy For Ext.Direct Comunication.
    /// Tha ProccessRequest Methode scanns all the available Assembly for Classes and Methods with the 
    /// Direct Attribute.
    /// </summary>
    public class DirectProxy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string routerUrl = ConfigurationCache.getRouterUrl();

            // set default namspace for the remoting API
            string apiNamespace = "Ext.app.REMOTING_API";
            if (context.Request.Form["ns"] != null)
            {
                // if there is an namespace parameter, use it...
                apiNamespace = context.Request.Form["ns"].ToString();
            }

            DirectProviderCache cache = DirectProviderCache.GetInstance();
            DirectProvider provider;

            //After being configured, the provider should be cached.
            if (!cache.ContainsKey(apiNamespace))
            {
                ;
                provider = new DirectProvider(apiNamespace, routerUrl);
                provider.Configure(AppDomain.CurrentDomain.GetAssemblies());
                cache.Add(apiNamespace, provider);
            }
            else
            {
                provider = cache[apiNamespace];
            }

            context.Response.Write(provider.ToString());

            /*
            old code..
           
            
            // set the Response typ to javascript since the responce gets Parsed into an Script Tag
            context.Response.ContentType = "text/JavaScript";

            string rem = "{";
            rem += "url: \""+routerUrl+"\",";
            rem += "type:\"remoting\",";


            //string json = DirectProxyGenerator.generateDirectApi();
           //rem += json;
            rem += "};";

            rem = apiNamespace + ".REMOTING_API =" + rem;
            rem = "(function(){" + rem + "})();";

            context.Response.Write(rem);
           */
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
