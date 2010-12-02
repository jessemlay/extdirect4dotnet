using System;
using System.Linq;
using System.Reflection;

namespace ExtDirect4DotNet {
    /// <summary>
    /// EventArgs object used to pass data for <see cref="DirectRouter.DirectMethodError"/> events.
    /// </summary>
    public class DirectMethodErrorEventArgs : EventArgs {
        private readonly DirectRequest _directRequest;

        private readonly TargetInvocationException _exception;

        private DirectResponse _directResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectMethodErrorEventArgs"/> class.
        /// </summary>
        /// <param name="directRequest">The <see cref="DirectRequest"/> being processed at the time of the exception.</param>
        /// <param name="exception">The exception.</param>
        public DirectMethodErrorEventArgs(DirectRequest directRequest, TargetInvocationException exception) {
            _directRequest = directRequest;
            _exception = exception;
        }

        /// <summary>
        /// Gets the <see cref="DirectRequest"/> being processed at the time of the exception.
        /// </summary>
        /// <value>The direct request.</value>
        public DirectRequest DirectRequest {
            get { return _directRequest; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DirectResponse"/> for the invocation of the DirectMethod.<br/>
        /// If the <see cref="DirectResponse"/> is not set, a default implementation will be used.
        /// </summary>
        /// <value>The direct response.</value>
        public DirectResponse DirectResponse {
            get {
                if (_directResponse == null) {
                    return new DirectResponse(DirectRequest, InnerException);
                }
                return _directResponse;
            }
            set { _directResponse = value; }
        }

        /// <summary>
        /// Gets the <see cref="TargetInvocationException"/> thrown from the invocation of the DirectMethod. <br/>
        /// The <see cref="InnerException"/> property is most likely what you will want to inspect.
        /// </summary>
        /// <value>The exception.</value>
        public TargetInvocationException Exception {
            get { return _exception; }
        }

        /// <summary>
        /// Gets the actual exception thrown from the invocation of the DirectMethod.
        /// This is the same as the InnerException of the <see cref="Exception"/> property.
        /// </summary>
        /// <value>The inner exception.</value>
        public Exception InnerException {
            get { return _exception.InnerException; }
        }
    }
}