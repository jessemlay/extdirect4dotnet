using System;
using System.Linq;
using System.Web;
using ExtDirect4DotNet.helper;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Represents the Proxy For Ext.Direct Comunication.
    /// Tha ProccessRequest Methode scanns all the available Assembly for Classes and Methods with the 
    /// Direct Attribute.
    /// </summary>
    public class DirectProxy : IHttpHandler {
        private static string url = "";

        public bool IsReusable {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context) {
            // set default namspace for the remoting API
            string apiNamespace = "Ext.app.REMOTING_API";
            if (context.Request.Form["ns"] != null) {
                // if there is an namespace parameter, use it...
                apiNamespace = context.Request.Form["ns"];
            }
            DirectProvider provider = getDirectProviderCache(apiNamespace);

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

        public static string routerUrl {
            get { return url; }
            set { url = value; }
        }

        public static DirectProvider getDirectProviderCache(string apiNameSpace) {
            string routerUrl = (DirectProxy.routerUrl == "") ? ConfigurationCache.getRouterUrl() : DirectProxy.routerUrl;

            // set default namspace for the remoting API
            string apiNamespace = (apiNameSpace == null || apiNameSpace == "") ? "Ext.app.REMOTING_API" : apiNameSpace;

            DirectProviderCache cache = DirectProviderCache.GetInstance();
            DirectProvider provider;

            //After being configured, the provider should be cached.
            if (!cache.ContainsKey(apiNamespace + "/" + routerUrl)) {
                provider = new DirectProvider(apiNamespace, routerUrl);
                provider.Configure(AppDomain.CurrentDomain.GetAssemblies());
                if (!cache.ContainsKey(apiNamespace + "/" + routerUrl)) {
                    cache.Add(apiNamespace + "/" + routerUrl, provider);
                }
            }
            else {
                provider = cache[apiNamespace + "/" + routerUrl];
            }
            return provider;
        }
    }
}