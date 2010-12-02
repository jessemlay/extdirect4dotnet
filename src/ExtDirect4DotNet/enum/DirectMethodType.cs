using System;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Enumeration to specify the functionality of the <see cref="DirectMethod"/>.
    /// TODO:Need to review the functionality of each enumeration to validate it's fully implemented and functional.
    /// </summary>
    public enum DirectMethodType {
        /// <summary>
        /// Contains a single Parameter with the Record to save.
        /// If the method has only One Paramter the Parametr gets deserialzed with the typ of that parameter.
        /// </summary>
        Create,

        /// <summary>
        /// Since The parameter will come as a Hash e.g.: {sort: 'name', sortDir: 'DESC'}
        /// the Hash will get mapted via Autoresolve.
        /// The Function should have at least those two Parameters: (string sort, string sortDir).
        /// </summary>
        Read,

        /// <summary>
        /// Len gets set to 2 since the Update function gets called with two parameters
        /// First is the Id of the Record, Second is the record with the updated data.
        /// If The DirectMethod it self has two parametrs, the Seconde Parameter in the Request
        /// Gets Serialized with the same type than the Function.
        /// </summary>
        Update,

        /// <summary>
        /// The Function will have only one Parameter the Id.
        /// </summary>
        Delete,

        /// <summary>
        /// Len gets set tu 2 since the Treeloader will send two values to the Server, the Node Id and an object of Parameter
        /// </summary>
        TreeLoad,

        /// <summary>
        /// Simple Function...
        /// </summary>
        Normal,

        /// <summary>
        /// Function with Form Specifix Handling (len gets set to 1 and the parameter will read out of the httpContext.Form)
        /// </summary>
        Form,

        /// <summary>
        /// This Method gets Handled as a normal Method but the Proxy will generate another Method with the posfix "_form"
        /// where len gets set to 1 and the Parameterhandling for this gets done as a Form Method
        /// </summary>
        Hybrid
    }
}