using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtDirect4DotNet.interfaces
{
    /// <summary>
    /// Implement this Interface to let the direct Processor call the afterCreation Methode directly after
    /// an Instance of this Direct Action has been created.
    /// 
    /// This is usefull to Implement some Class wide member init mechanism
    /// </summary>
    /// <typeparam name="T">The Type to use for the parameter while calling afterCreation. Possible Types are: DirectRequest, HttpContext, IList or any Serializble Type</typeparam>
    public interface IActionWithAfterCreation<T> 
    {
        /// <summary>
        /// This Method gets Called first during the lifetime of a DirectAction. 
        /// </summary>
        /// <param name="parameter">Can be any Extractble part of the Request 
        ///     DirectRequest: If the Type is DirectRequest, this method gets called with the first DirectRequest that wants to use this Direct Action
        ///     HttpContext: If the Type is HttpContex, this method gets calle with the HttpContext of the current Request
        ///     IList: If the Type is an Implement of IList the Parameters of the first DirectMethod get serialized as IList 
        ///     Any Other Serializble Type: If the Type is none of the first three The First Parameter of the first method using this DirectAction gets Serialized into that type.
        /// </param>
        void afterCreation(T parameter);
    
    }
}
