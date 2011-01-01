using System;
using System.Linq;
using System.Web;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Used by ExtDirect4DotNet to provide access to the current <see cref="HttpContext"/>.
    /// </summary>
    public interface IDirectAction {
        /// <summary>
        /// Gets or sets the current <see cref="HttpContext"/>.
        /// </summary>
        /// <value>
        /// The current <see cref="HttpContext"/>.
        /// </value>
        HttpContext CurrentHttpContext { get; set; }
    }
}