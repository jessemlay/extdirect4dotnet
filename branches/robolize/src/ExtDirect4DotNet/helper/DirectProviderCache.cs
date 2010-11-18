using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Simple cache for Ext.Direct providers
    /// </summary>
    public class DirectProviderCache : Dictionary<string, DirectProvider> {
        private static readonly DirectProviderCache instance = new DirectProviderCache();

        private Dictionary<string, DirectProviderCache> providers;

        private DirectProviderCache() {
            providers = new Dictionary<string, DirectProviderCache>();
        }

        /// <summary>
        /// Gets the singleton instance of this object.
        /// </summary>
        /// <returns>The DirectProviderCache instance.</returns>
        public static DirectProviderCache GetInstance() {
            return instance;
        }
    }
}