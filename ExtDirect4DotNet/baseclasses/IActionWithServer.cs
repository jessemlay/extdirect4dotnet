using System;
using System.Linq;
using System.Web;

namespace ExtDirect4DotNet {
    public interface IActionWithServer {
        void SetServer(HttpServerUtility server);
    }
}