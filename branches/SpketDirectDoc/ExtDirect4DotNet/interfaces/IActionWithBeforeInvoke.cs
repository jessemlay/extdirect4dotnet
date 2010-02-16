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
        /// <summary>
        /// Implement this Method for logic that should happen just before a method gets called
        /// </summary>
        /// <param name="methodType">The Type of the Method which gets called next</param>
        /// <param name="methodName">The Name of the Method which gets called next</param>
        /// <param name="parameter">The Parameters the method will be invoked with.</param>
        void beforeMethodInvoke(DirectMethodType methodType, string methodName, T parameter);
    }
}
