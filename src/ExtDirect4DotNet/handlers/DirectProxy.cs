using System;
using System.Linq;
using System.Web;
using ExtDirect4DotNet.helper;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Implementation of <see cref="IHttpHandler"/> responsible for managing methods configured for Ext.Direct communication.<br/>
    /// This handler will output the complete list of methods in json format.
    /// The output will be cached for the life of the application.<br/>
    /// Only methods marked with the <see cref="DirectMethod"/> attribute within a class marked with the <see cref="DirectAction"/> attribute.
    /// </summary>
    public class DirectProxy : IHttpHandler {
        ///<summary>
        /// Value is "Ext.app.REMOTING_API" and is used as default namespace the API.
        ///</summary>
        public const string DEFAULT_API_NAMESPACE = "Ext.app.REMOTING_API";

        static DirectProxy() {
            routerUrl = string.Empty;
        }

        public bool IsReusable {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context) {
            string customNamespace = context.Request.Form["ns"];
            if (string.IsNullOrEmpty(customNamespace)) {
                customNamespace = DEFAULT_API_NAMESPACE;
            }

            DirectProvider provider = GetDirectProviderCache(customNamespace);
            context.Response.Write(provider.ToString());
        }

        /// <summary>
        /// Gets or sets the router URL.
        /// </summary>
        /// <value>The router URL.</value>
        public static string routerUrl { get; set; }

        /// <summary>
        /// Gets the cached <see cref="DirectProvider"/> using the <see cref="DEFAULT_API_NAMESPACE"/>.
        /// </summary>
        /// <returns></returns>
        public static DirectProvider GetDirectProviderCache() {
            return GetDirectProviderCache(DEFAULT_API_NAMESPACE);
        }

        /// <summary>
        /// Gets the cached <see cref="DirectProvider"/> using the provided API Namespace.
        /// </summary>
        /// <param name="apiNamespace">The API Namespace.</param>
        /// <returns></returns>
        public static DirectProvider GetDirectProviderCache(string apiNamespace) {
            if (string.IsNullOrEmpty(apiNamespace)) {
                throw new ArgumentException("An API Namespace must be provided.", "apiNamespace");
            }

            string rUrl = string.IsNullOrEmpty(routerUrl) ? ConfigurationCache.GetRouterUrl() : routerUrl;
            DirectProviderCache cache = DirectProviderCache.GetInstance();
            DirectProvider provider;

            //After being configured, the provider should be cached.
            if (!cache.ContainsKey(apiNamespace + "/" + rUrl)) {
                provider = new DirectProvider(apiNamespace, rUrl);
                provider.Configure(AppDomain.CurrentDomain.GetAssemblies());
                if (!cache.ContainsKey(apiNamespace + "/" + rUrl)) {
                    cache.Add(apiNamespace + "/" + rUrl, provider);
                }
            }
            else {
                provider = cache[apiNamespace + "/" + rUrl];
            }
            return provider;
        }
    }
}