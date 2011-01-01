using System;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// This attribute should be added to methods that will be Ext.direct methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DirectMethodAttribute : Attribute {
        /// <summary>
        /// Set this Option to specify this Method for the CRUD Actions
        /// </summary>
        public DirectMethodType MethodType = DirectMethodType.Normal;

        /// <summary>
        /// Set this Option for Specific output Handling
        /// </summary>
        public OutputHandling OutputHandling = OutputHandling.Normal;

        /// <summary>
        /// Set this Option for specific parameter-mapping handling
        /// This may influnce the len propertie of the Function
        /// </summary>
        public ParameterHandling ParameterHandling = ParameterHandling.PassThrough;

        /// <summary>
        /// Set this to true to let ExtDirect4DotNet try to SerializeParameter Types for you.
        /// 
        /// This Option makes is Usefull when you want to Call a method with a Date for Example
        /// The Router will use the Newtonsofts deserialize Function and trys to deserialize the Json string into 
        /// The needed parameter
        /// </summary>
        public bool SerializeParameterTypes;
    }
}