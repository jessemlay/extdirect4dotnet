using System;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Thrown when an error occurs during a Ext.Direct call.
    /// </summary>
    public class DirectException : ApplicationException {
        public uint errorCode;

        public DirectException() {
        }

        public DirectException(string msg)
            : base(msg) {
        }

        public DirectException(string msg, DirectRequest directRequest)
            : base(msg + " DirectAction: " + directRequest.Action + " DirectMethod " + directRequest.Method) {
        }
    }

    /// <summary>
    /// Thrown when an error occurs during a Ext.Direct call.
    /// </summary>
    public class DirectParameterException : DirectException {
        public DirectParameterException() {
        }

        public DirectParameterException(string msg, DirectRequest directRequest)
            : base(msg, directRequest) {
        }
    }
}