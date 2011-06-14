using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace ExtDirect4DotNet.parametermapper
{
    /// <summary>
    /// Represents the Base Class you have to extend from to build your own ParameterMapper Logic
    /// </summary>
    public abstract class ParameterMapperBase : IParameterMapper
    {

        #region IParameterMapper Member

        public abstract object[] MapParameter(ParameterInfo[] methodParameter, DirectRequest directRequest);

        public Hashtable getInvalidParameterMessage(DirectRequest directRequest, ParameterInfo paramInfo, Object value)
        {
            Hashtable errorHash = new Hashtable();
            errorHash.Add("id", paramInfo.Name);
            errorHash.Add("msg", "Couldn't Serialize the value (\"" +value.ToString()+ "\") to the parameter (\"" + paramInfo.Name + "\") of the type " + paramInfo.ParameterType.ToString());
            return errorHash;
        }

        #endregion
    }
}
