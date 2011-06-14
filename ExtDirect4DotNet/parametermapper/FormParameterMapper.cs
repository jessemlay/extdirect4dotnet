using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using Newtonsoft.Json;
using ExtDirect4DotNet.exceptions;

namespace ExtDirect4DotNet.parametermapper
{
    /// <summary>
    /// Mapper For DirectRequets with isForm == true
    /// Maps the 
    /// </summary>
    public class FormParameterMapper : ParameterMapperBase
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

            // will get filled with the paramters the function gets called with
            Object[] paramMap = new object[parmInfo.Length];

            // first of all check if the first Parameters type is HttpRequest, if so just pass in the
            // HttpRequest Object from the current HttpContext as the first Parameter
            if (parmInfo[0].ParameterType == directRequest.HttpContext.Request.GetType())
            {
                paramMap[0] = directRequest.HttpContext.Request;
            }
            else
            {
                // if the first Parameter is not HttpContext, the Funktion wants to get the Parameter mapped
                HttpRequest orginalRequest = directRequest.HttpContext.Request;
                int i2 = 0;
                // try to find Parameters in the Formvariables
                foreach (var parm in parmInfo)
                {
                    // skip all HttpPosted Files (get mapped later)
                    Type type = parmInfo[i2].ParameterType;
                    if (type.Name == "HttpPostedFile")
                    {
                        i2++;
                        continue;
                    }
                    String curentParameter = orginalRequest.Form[parm.Name];
                    // check if the submited form sent the parameter, if not skip it
                    if (curentParameter == null)
                    {
                        i2++;
                        continue;
                    }
                    // we can add the parameter to param Map now if we are able to deserialize it
                    // so lets try
                    try
                    {
                        // if the parameter type we want to map the parameter from the request to is
                        // Object or String we can use it as is.
                        if (type == System.Type.GetType("System.Object") || type == System.Type.GetType("System.String"))
                        {
                            paramMap[i2] = curentParameter;
                        }
                        else
                        {
                            // if the parameter is not string or Object we will have to serialize it...
                            paramMap[i2] = JsonConvert.DeserializeObject(curentParameter.ToString(), type);
                        }
                    }
                    catch (Exception e) {
                        errors.Add(this.getInvalidParameterMessage(directRequest, parmInfo[i2], curentParameter));                        
                    }

                    i2++;
                }

                i2 = 0;
                // try to find matching Parameters in the Files-list
                foreach (var parm in parmInfo)
                {

                    Type type = parmInfo[i2].ParameterType;
                    if (((HttpRequest)orginalRequest).Files[parm.Name] != null)
                    {
                        if (type.Name != "HttpPostedFile")
                        {
                            // FIX use the filename instead of "File"
                            errors.Add(this.getInvalidParameterMessage(directRequest, parmInfo[i2], "File"));
                        
                        }
                        paramMap[i2] = ((HttpRequest)orginalRequest).Files[parm.Name];
                    }
                    i2++;
                }

            }

            if (errors.Count > 0)
            {
                throw new DirectFormInvalidException(directRequest, errors);
                        
            }
            return paramMap;
        }

        #endregion


    }
}
