using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ExtDirect4DotNet
{
    public interface IActionWithServer
    {
         void SetServer(HttpServerUtility server);
    }
}
