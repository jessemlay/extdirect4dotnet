using System;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Thrown when an error occurs during a Ext.Direct call.
    /// </summary>
    public class DirectException : ApplicationException {
        /*/// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public uint ErrorCode { get; set; }*/

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectException"/> class.
        /// </summary>
        public DirectException() : base("An exception occurred in the ExtDirect4DotNet library.") {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public DirectException(string message) : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="directRequest">The direct request.</param>
        public DirectException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}