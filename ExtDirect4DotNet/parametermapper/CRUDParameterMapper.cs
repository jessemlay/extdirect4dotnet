using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ExtDirect4DotNet.exceptions;

namespace ExtDirect4DotNet.parametermapper
{
    /// <summary>
    /// (Since a Update Function has another parameter Structure we need our own resolve logic function for it)
    /// Resolves the parameter of an UpdateRequest to the methods Attributes
    /// 
    /// (This Mapper also works for the tree sice the structure is the same...)
    /// </summary>
    public class CUDParameterMapper : ParameterMapperBase
    {

        #region IParameterMapper Member

        public object[] MapParameter(System.Reflection.ParameterInfo[] parmInfo, DirectRequest directRequest, string rootParameter)
        {
            // create the parameter object
            Object[] paramMap = new object[parmInfo.Length];
            JArray parameter = ((JArray)directRequest.Data);

            // since astore sends its params next to the the object we want to execute the crud on
            // we have to extract it here
            JObject allParams = (JObject)parameter[0];
            // extract the  crud object
            Object crudObject = (Object)allParams[rootParameter];



            // okay use the name resover to resolve the additional parameters the store may sent
            NameParameterMapper nameMapper = new NameParameterMapper();
            paramMap = nameMapper.MapParameter(parmInfo, directRequest, allParams);

            Object crudObj = null;

            // okay now try to serialize the CRUD object and map it to the first parameter
            
            try
            {
                if (parmInfo[0].ParameterType == System.Type.GetType("System.Object"))
                {
                    crudObj = crudObject;
                }
                else
                {
                    crudObj = JsonConvert.DeserializeObject(crudObject.ToString(), parmInfo[0].ParameterType);
                }
            }
            catch (Exception e)
            {
                ArrayList errors = new ArrayList();
                errors.Add(getInvalidParameterMessage(directRequest, parmInfo[0], parameter[0]));
                throw new DirectParameterException(directRequest, errors);

            }

            paramMap[0] = crudObj;
            return paramMap;

        }

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

            JArray parameter = ((JArray)directRequest.Data);
            if (parameter != null)
            {
                if (parameter.Count < 2)
                {
                    throw new DirectParameterException("Tried to call an Updatemethod with less than 2 Parameters.", directRequest);
                }
                Type paramTyp = parmInfo[0].ParameterType;

                Object id = new Object();
                //paramMap[0] = ((paramTyp)parameter[0]);
                if (parameter[0] is JValue)
                {

                    try
                    {
                        if (paramTyp == System.Type.GetType("System.Object"))
                        {
                            id = parameter[0];
                        }
                        else
                        {
                            id = JsonConvert.DeserializeObject(parameter[0].ToString(), paramTyp);
                        }
                    }
                    catch (Exception e)
                    {
                        errors.Add(getInvalidParameterMessage(directRequest, parmInfo[0], parameter[0]));
                        
                    }

                }

                JArray tempParams = new JArray();

                tempParams.Add(parameter[1]);

                // use the name Parameter Mapper to map the actual data
                NameParameterMapper nameMapper = new NameParameterMapper();

                paramMap = nameMapper.MapParameter(parmInfo, directRequest, tempParams);

                paramMap[0] = id;

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
