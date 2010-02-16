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
        /// Implement this method to do some after invokation logic.
        /// This Method gets called directly after a Direct Method call.
        /// </summary>
        /// <param name="methodType">The DirectMethodType of the previosly called Mathod</param>
        /// <param name="methodName">The Name of the Method that was perviosly called</param>
        /// <param name="theReturnedObject">The Object returned by the Methode called before</param>
        Object afterMethodInvoke(DirectMethodType methodType, string methodName, Object theReturnedObject);
    }
}
