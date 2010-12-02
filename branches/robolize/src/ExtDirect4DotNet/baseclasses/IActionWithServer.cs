using System;
using System.Linq;
using System.Web;

namespace ExtDirect4DotNet {
    //TODO:Merge functionality with IActionWithSessionState.
    public interface IActionWithServer {
        void SetServer(HttpServerUtility server);
    }
}