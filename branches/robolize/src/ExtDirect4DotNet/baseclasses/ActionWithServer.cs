using System;
using System.Linq;
using System.Web;

namespace ExtDirect4DotNet {
    public class ActionWithServer : IActionWithServer {
        public HttpServerUtility Server;

        public void SetServer(HttpServerUtility server) {
            Server = server;
        }
    }
}