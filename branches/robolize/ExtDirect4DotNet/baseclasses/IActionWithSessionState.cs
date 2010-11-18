using System;
using System.Linq;
using System.Web.SessionState;

namespace ExtDirect4DotNet {
    public interface IActionWithSessionState {
        void SetSession(HttpSessionState session);
    }
}