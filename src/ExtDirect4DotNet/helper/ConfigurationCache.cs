using System;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using log4net;

namespace ExtDirect4DotNet.helper {
    //TODO:This class could be removed.  It only needs to resolve a path to the router.  If it stays, it doesn't need to be static and there is no caching going on here.
    [Obsolete("This class could be removed.")]
    public static class ConfigurationCache {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ConfigurationCache));

        private static string cachedRouterUrl = "";

        /// <summary>
        /// Trys to find the configured handler for the router. Cache is not cleared when calling this method.
        /// </summary>
        /// <returns>Returns the url to the Ext Direct Router.</returns>
        public static string GetRouterUrl() {
            return GetRouterUrl(false);
        }

        /// <summary>
        /// Trys to find the configured handler for the router.
        /// </summary>
        /// <param name="clear">Clear Cache and reload config.</param>
        /// <returns>Returns the url to the Ext Direct Router.</returns>
        public static string GetRouterUrl(bool clear) {
            //TODO:This if will never evaluate to true.
            if (!clear && cachedRouterUrl != "") {
                return cachedRouterUrl;
            }

            string routerUrl = "";

            // readout the system.web/httpHandlers secion in the webconfig this dll is bound. 
            // This is needed to mapp to the correct router url...
            HttpHandlersSection httpHandlers = ConfigurationManager.GetSection("system.web/httpHandlers") as HttpHandlersSection;

            // now itterate over all the available actions and find the one containing the url.
            if (httpHandlers != null) {
                foreach (HttpHandlerAction curHandler in httpHandlers.Handlers) {
                    // search for the handler link to the router class.
                    //TODO:Need to verify this will always resolve the correct path to the router. Path could be wrong depending on site configuration in IIS.
                    if (curHandler.Type.IndexOf("ExtDirect4DotNet.DirectRouter") != -1) {
                        routerUrl = curHandler.Path;
                        break;
                    }
                }
            }

            if (routerUrl == "") {
                DirectConfigurationException exception = new DirectConfigurationException();
                Logger.Error("routerUrl must be set.", exception);
                throw exception;
            }

            return routerUrl;
        }
    }
}