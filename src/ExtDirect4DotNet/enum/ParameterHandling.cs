using System;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Enumeration to specify the processing parameters of the <see cref="DirectMethod"/>.
    /// TODO:Need to review the functionality of each enumeration to validate it's fully implemented and functional.
    /// </summary>
    public enum ParameterHandling {
        /// <summary>
        /// No Special Handling will be done
        /// The Parameter mapping been done by parameter Position 
        /// </summary>
        PassThrough,

        /// <summary>
        /// Set this to true to set len propertie to for normal Methodes (No CRUD Methods special handling) to 1 and try to resolve the parameter.
        /// This makse it possible to submit paramers independend from their order same technic as in exct used 
        /// minimize paramter to one containing all the properties.
        /// 
        /// </summary>
        /// <example>
        /// {name: 'Martin', lastname: 'Spaeth'} will execute the following sample method.
        /// 
        /// [DirectMethod(ResolveParameterNames=true)]
        /// public string sample(string name, string lastname){
        ///    return "{name:"+name+",lastname:"+lastname"}";
        /// }
        /// 
        /// will return an API definition like that:
        /// 
        /// "ActionName":[{"name":"sample","len":1,"formHandler":false}
        /// 
        /// And can get called from Ext like that:
        /// 
        /// ActionName.sample({lastname:'spaeth',name:'martin'});
        /// </example>
        AutoResolve
    }
}