using System;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// This attribute should be added to Ext.direct classes that to indicate they will participate in routing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DirectActionAttribute : Attribute {
    }
}