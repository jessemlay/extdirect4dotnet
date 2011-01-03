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
        public DirectParameterException(string message) : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectParameterException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DirectParameterException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}