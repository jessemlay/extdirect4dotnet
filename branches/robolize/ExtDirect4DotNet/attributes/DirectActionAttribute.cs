using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtDirect4DotNet
{
    /// <summary>
    /// This attribute should be added to Ext.direct classes that to indicate they will participate in routing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DirectActionAttribute : Attribute
    {
    }
}
