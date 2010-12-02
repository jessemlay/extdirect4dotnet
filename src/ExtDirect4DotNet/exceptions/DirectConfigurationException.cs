using System;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// The exception thrown when the configured handler for the router cannot be found.
    /// </summary>
    public class DirectConfigurationException : DirectException {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectConfigurationException"/> class.
        /// </summary>
        public DirectConfigurationException() :
            base("Configuration for the DirectRouter wasn't found. Please add e.g.<add verb=\"*\" path=\"directRouter.rfc\" type=\"ExtDirect4DotNet.DirectProxy,ExtDirect4DotNet\" /> to your web.config.") {
        }
    }
}