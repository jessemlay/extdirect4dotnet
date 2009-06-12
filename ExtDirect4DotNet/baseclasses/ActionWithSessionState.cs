using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace ExtDirect4DotNet
{
    public class ActionWithSessionState : IActionWithSessionState
    {
        public HttpSessionState Session;

        #region IActionWithSessionState Member

        public void SetSession(HttpSessionState session)
        {
            Session = session;
        }

        #endregion

       
    }
}
