using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ExtDirect4DotNet {
    /// <summary>
    /// A base implementation of <see cref="IDirectAction"/>.<br/>
    /// The <see cref="DirectActionAttribute"/> is already applied to this class so derived classes are not required apply the <see cref="DirectActionAttribute"/>.
    /// </summary>
    [DirectAction]
    public class DirectActionBase : IDirectAction {
        /// <summary>
        /// Gets or sets the current <see cref="HttpContext"/>.
        /// </summary>
        /// <value>
        /// The current <see cref="HttpContext"/>.
        /// </value>
        public HttpContext CurrentHttpContext { get; set; }

        /// <summary>
        /// Gets the <see cref="HttpServerUtility"/> object that provides methods used in processing Web requests.
        /// </summary>
        public HttpServerUtility Server {
            get { return CurrentHttpContext.Server; }
        }

        /// <summary>
        /// Gets the <see cref="HttpSessionState"/> object for the current HTTP request.
        /// </summary>
        public HttpSessionState Session {
            get { return CurrentHttpContext.Session; }
        }
    }
}