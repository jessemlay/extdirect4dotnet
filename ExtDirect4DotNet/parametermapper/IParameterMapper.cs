using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace ExtDirect4DotNet.parametermapper
{
    interface IParameterMapper
    {
        /// <summary>
        /// Trys to map the Datat from the directRequest to the methodParameter
        /// </summary>
        /// <param name="methodParameter">The ParameterInfo to map the DirectRequest to</param>
        /// <param name="directRequest">The DirecRequest containing the data we want to map to the Method</param>
        /// <returns>An Array of Object wich represents the  Parameter to call the Method with</returns>
        Object[] MapParameter(ParameterInfo[] methodParameter, DirectRequest directRequest);

        Hashtable getInvalidParameterMessage(DirectRequest directRequest, ParameterInfo paramInfo, Object value);
    }
}
