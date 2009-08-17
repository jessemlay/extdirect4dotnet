using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Configuration;

namespace ExtDirect4DotNet.helper
{
    public static class ConfigurationCache
    {
        static private string cachedRouterUrl = "";

        static public string getRouterUrl() 
        {
            return getRouterUrl(false);
        }

        /// <summary>
        /// trys to find the configured handler for the router
        /// </summary>
        /// <param name="clear">clear Cache and reload config</param>
        /// <returns>returns the url to the Ext Direct Router</returns>
        static public string getRouterUrl(bool clear)
        {
            if (!clear && cachedRouterUrl != "")
            {
                return cachedRouterUrl;
            }
            string routerUrl = "";

            // readout the system.web/httpHandlers secion in the webconfig this dll is bound. 
            // This is needed to mapp to the correct router url...
            HttpHandlersSection httpHandlers = ConfigurationManager.GetSection("system.web/httpHandlers") as HttpHandlersSection;

            // now itterate over all the available actions and find the one containing the url.
            foreach (HttpHandlerAction curHandler in httpHandlers.Handlers)
            {
                // search for the handler link to the router class.
                if (curHandler.Type.IndexOf("ExtDirect4DotNet.DirectRouter") != -1)
                {
                    routerUrl = curHandler.Path;
                    break;
                }
            }

            if (routerUrl == "")
            {
                throw new Exception("Configuration for the DirectRouter wasn't found. "
                    + "Please add e.g.<add verb=\"*\" path=\"directRouter.rfc\" type=\"ExtDirect4DotNet.DirectProxy,ExtDirect4DotNet\" /> to your web.config"
                );
            }

            return routerUrl;
        }
    }
}
