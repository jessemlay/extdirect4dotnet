using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace ExtDirect4DotNet
{
    public class ActionWithServer : IActionWithServer
    {
        public HttpServerUtility Server;

        #region IActionWithServer Member

        public void SetServer(HttpServerUtility server)
        {
            Server = server;
        }

        #endregion
    }
}
