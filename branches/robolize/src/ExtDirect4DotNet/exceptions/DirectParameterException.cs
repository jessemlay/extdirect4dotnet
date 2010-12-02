using System;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Thrown when an error occurs during a Ext.Direct call.
    /// </summary>
    public class DirectParameterException : DirectException {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectParameterException"/> class.
        /// </summary>
        public DirectParameterException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectParameterException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="directRequest">The direct request.</param>
        public DirectParameterException(string message, DirectRequest directRequest) : base(message, directRequest) {
        }
    }
}