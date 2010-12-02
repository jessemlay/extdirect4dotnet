using System;
using System.Linq;
using System.Web.SessionState;

namespace ExtDirect4DotNet {
    //TODO:Merge functionality with IActionWithServer.
    public interface IActionWithSessionState {
        void SetSession(HttpSessionState session);
    }
}