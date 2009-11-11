using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using ExtDirect4DotNet.exceptions;

namespace ExtDirect4DotNet.parametermapper
{
    /// <summary>
    /// Mapper That Maps the Parameter by the Parameternames
    /// Maps the 
    /// </summary>
    public class NameParameterMapper : ParameterMapperBase
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



            JArray parameter = ((JArray)directRequest.Data);
            return MapParameter(parmInfo, directRequest, parameter);

        }

        /// <summary>
        /// Maps the given jsonObject into an Object of parameters 
        /// </summary>
        /// <param name="paramInfo">The Array of Parameter that describes the arguments to map to</param>
        /// <param name="jsonObject">the Json Object which should get mapped to the arguments</param>
        /// <returns>returns an Object array to use in a function call as parameters.</returns>
        public object[] MapParameter(System.Reflection.ParameterInfo[] paramInfo, DirectRequest directRequest, JObject jsonObject)
        {
            ArrayList errors = new ArrayList();

            // will get filled with the paramters the function gets called with
            object[] paramMap = new object[paramInfo.Length];
            int i = 0;
            foreach (ParameterInfo param in paramInfo)
            {
                
               Object toMap = jsonObject[param.Name];
               if (toMap != null)
               {
                   try
                   {
                       if (param.ParameterType == System.Type.GetType("System.Object"))
                       {
                           paramMap[i] = toMap;
                       }
                       else
                       {
                           paramMap[i] = JsonConvert.DeserializeObject(toMap.ToString(), param.ParameterType);
                       }
                   }
                   catch (Exception e)
                   {
                       errors.Add(this.getInvalidParameterMessage(directRequest, paramInfo[i], toMap));

                   }
               }
               i++;
                    
            }
            if (errors.Count > 0)
            {
                throw new DirectParameterException(directRequest, errors);

            }
            return paramMap;

        }

        public object[] MapParameter(System.Reflection.ParameterInfo[] parmInfo,  DirectRequest directRequest, JArray parameter)
        {


            ArrayList errors = new ArrayList();

            // will get filled with the paramters the function gets called with
            Object[] paramMap = new object[parmInfo.Length];

            if (parameter != null)
            {
                for (int i = 0; i < parameter.Count; i++)
                {
                    Object curParam = parameter[i];
                    if (curParam is Newtonsoft.Json.Linq.JObject)
                    {
                        int i2 = 0;

                        foreach (var parm in parmInfo)
                        {
                            if (((Newtonsoft.Json.Linq.JObject)curParam)[parm.Name] != null)
                            {
                                Object curentParameter = ((Newtonsoft.Json.Linq.JObject)curParam)[parm.Name];

                                if (curentParameter != null)
                                {

                                    Type type = parmInfo[i2].ParameterType;
                                    try
                                    {
                                        if (type == System.Type.GetType("System.Object"))
                                        {
                                            paramMap[i2] = curentParameter;
                                        }
                                        else
                                        {
                                            paramMap[i2] = JsonConvert.DeserializeObject(curentParameter.ToString(), type);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        errors.Add(this.getInvalidParameterMessage(directRequest, parmInfo[i2], curentParameter));
                        
                                    }
                                }
                            }
                            i2++;
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                throw new DirectParameterException(directRequest, errors);
                        
            }
            return paramMap;
        }

        #endregion


    }
}
