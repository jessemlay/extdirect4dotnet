using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ExtDirect4DotNet.parametermapper
{
    /// <summary>
    /// Mapper For DirectRequets with isForm == true
    /// Maps the 
    /// </summary>
    public class IndexParameterMapper : ParameterMapperBase
    {

        #region IParameterMapper Member

        /// <summary>
        
        /// Mapped den übergebenen directRequest auf die übergebenen methodenParmeter 
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <param name="directRequest"></param>
        /// <returns></returns>
        public override object[] MapParameter(System.Reflection.ParameterInfo[] parmInfo, DirectRequest directRequest)
        {
            ArrayList errors = new ArrayList();

            Object[] paramMap = new object[parmInfo.Length];

            if (!(directRequest.Data is JValue))
            {

                JArray parameter = ((JArray)directRequest.Data);
                int i = 0;
                // use the Position as Indicator
                for (i = 0; i < parameter.Count && i < paramMap.Length; i++)
                {

                    Type type = parmInfo[i].ParameterType;

                    try
                    {
                        if (type == System.Type.GetType("System.Object"))
                        {
                            paramMap[i] = parameter[i];
                        }
                        else
                        {
                            paramMap[i] = JsonConvert.DeserializeObject(parameter[i].ToString(), type);
                        }


                    }
                    catch (Exception e)
                    {
                        errors.Add(getInvalidParameterMessage(directRequest, parmInfo[i], parameter[i]));
                        
                    }

                }
            }
            return paramMap;
        }

        #endregion


    }
}
