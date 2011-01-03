using System.Web;
using System.Web.SessionState;
using ExtDirect4DotNet;

namespace ExtDirect4DotNetSample {
    [DirectAction]
    public class DirectActionBase {
        /// <summary>
        /// Gets the <see cref="HttpServerUtility"/> object that provides methods used in processing Web requests.
        /// </summary>
        public HttpServerUtility Server {
            get { return HttpContext.Current.Server; }
        }

        /// <summary>
        /// Gets the <see cref="HttpSessionState"/> object for the current HTTP request.
        /// </summary>
        public HttpSessionState Session {
            get { return HttpContext.Current.Session; }
        }
    }
}