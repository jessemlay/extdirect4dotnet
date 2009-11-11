using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtDirect4DotNet.interfaces
{
    /// <summary>
    /// Implement this class to 
    /// </summary>
    public interface IActionWithAfterInvoke
    {
        /// <summary>
        /// Implement this method to do some after invokation logic
        /// </summary>
        /// <param name="theReturnedObject"></param>
        void afterMethodInvoke(string methodName, Object theReturnedObject);
    }
}
