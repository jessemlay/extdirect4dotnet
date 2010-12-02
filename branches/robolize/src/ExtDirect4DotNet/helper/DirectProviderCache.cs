using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Simple cache for Ext.Direct providers
    /// </summary>
    public class DirectProviderCache : Dictionary<string, DirectProvider> {
        private static readonly DirectProviderCache Instance = new DirectProviderCache();

        private DirectProviderCache() {
        }

        /// <summary>
        /// Gets the singleton instance of this object.
        /// </summary>
        /// <returns>The DirectProviderCache instance.</returns>
        public static DirectProviderCache GetInstance() {
            return Instance;
        }
    }
}