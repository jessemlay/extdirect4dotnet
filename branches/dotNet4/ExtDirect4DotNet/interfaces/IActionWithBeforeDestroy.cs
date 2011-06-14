using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ExtDirect4DotNet.interfaces
{
    /// <summary>
    /// Implement this Interface to add some beforere destroy Logic to the class
    /// The beforeDestroy methode gets called on all Actions that implement this Interface
    /// just before the server response. This may the place to close a Database connection.
    /// </summary>
    public interface IActionWithBeforeDestroy
    {
        /// <summary>
        /// gets called before the Action gets destroyed
        /// </summary>
        /// <param name="parameter">The HttpContext this Action was created in.</param>
        void beforeDestroy(HttpContext parameter);
    }
}
