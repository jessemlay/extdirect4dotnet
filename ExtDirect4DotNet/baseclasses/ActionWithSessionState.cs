using System;
using System.Linq;
using System.Web.SessionState;

namespace ExtDirect4DotNet {
    public class ActionWithSessionState : IActionWithSessionState {
        public HttpSessionState Session;

        public void SetSession(HttpSessionState session) {
            Session = session;
        }
    }
}