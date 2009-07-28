using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using ExtDirect4DotNet;

namespace WebApplication1
{
   /// <summary>
    /// Zusammenfassungsbeschreibung für $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class directProxy : DirectProxy
    {

        public directProxy()
        {
            DirectProxy.routerUrl = "directRouter.ashx";
        }

    }
}

