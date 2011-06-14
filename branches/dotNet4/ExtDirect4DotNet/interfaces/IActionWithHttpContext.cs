using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ExtDirect4DotNet.baseclasses
{
    interface IActionWithHttpContext
    {
        void SetHttpContext(HttpContext httpContext);
    }
}
