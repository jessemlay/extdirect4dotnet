using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtDirect4DotNet.interfaces
{
    /// <summary>
    /// Implement this Interface to let the processor call the beforeMethod Call before every Request
    /// </summary>
    public interface IActionWithBeforeInvoke<T>
    {
        void beforeMethodInvoke(DirectMethodType methodType, string methodName, T parameter);
    }
}
