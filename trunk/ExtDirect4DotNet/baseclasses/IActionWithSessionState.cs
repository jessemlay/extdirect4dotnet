using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace ExtDirect4DotNet
{
    public interface IActionWithSessionState
    {
         void SetSession(HttpSessionState session);
    }
}
