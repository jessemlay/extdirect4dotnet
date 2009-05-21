using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using ExtDirect4DotNet.helper;
using System.Web.SessionState;

namespace ExtDirect4DotNet
{
    internal class DirectMethod
    {


        internal DirectMethod(MethodInfo method, DirectMethodType methType, DirectAction parentAction)
            : this( method,  methType,  parentAction, method.Name)
        {
             


            // this.Parameters = method.GetParameters().Length;
        }

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="method">The method information.</param>
        internal DirectMethod(MethodInfo method, DirectMethodType methType, DirectAction parentAction, string directMethodName)
        {
            this.ParentAction = parentAction;
            this.MethodType = methType;
            this.Method = method;
            this.IsForm = (methType == DirectMethodType.Form); // FIX Utility.HasAttribute(method, typeof(DirectMethodFormAttribute));
            this.Name = directMethodName;

            this.MethodAttributes  = ((DirectMethodAttribute)this.Method.GetCustomAttributes(typeof(DirectMethodAttribute), true)[0]);
                
            
            // this.Parameters = method.GetParameters().Length;
        }

        /// <summary>
        /// Gets the method info.
        /// </summary>
        internal MethodInfo Method
        {
            get;
            private set;
        }

        internal DirectMethodAttribute MethodAttributes
        {
            get;
            set;
        }

        internal DirectAction ParentAction
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether the method is a form method;
        /// </summary>
        internal bool IsForm
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        internal string Name
        {
            get;
            private set;
        }

        internal DirectMethodType MethodType;

        /// <summary>
        /// Gets the number of parameters for the method.
        /// </summary>
        internal int Parameters
        {
            get 
            {

                if (this.MethodAttributes.ParameterHandling == ParameterHandling.PassThrough &&
                    this.MethodType == DirectMethodType.Normal)
                {
                    return this.Method.GetParameters().Length;                   
                }
                else if(this.MethodAttributes.ParameterHandling == ParameterHandling.AutoResolve)
                {
                    switch (this.MethodType)
                    {
                        case DirectMethodType.Normal:
                            return 1;

                    }
                }
                switch (this.MethodType)
                {
                    case DirectMethodType.Create:
                        return 1;
                    case DirectMethodType.Read:
                        return 1;
                    case DirectMethodType.Update:
                        return 2;
                    case DirectMethodType.Delete:
                        return 1;
                    case DirectMethodType.Form:
                        return 1;
                } 
                return this.Method.GetParameters().Length;
            }
        }

        internal OutputHandling OutputHandling
        {
            get
            {
                DirectMethodAttribute da = ((DirectMethodAttribute)this.Method.GetCustomAttributes(typeof(DirectMethodAttribute), true)[0]);
                return da.OutputHandling;
            }
        }

        private Object[] ResolveParametersByIndex(DirectRequest directRequest)
        {
            ParameterInfo[] parmInfo = this.Method.GetParameters();
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
                            } else {
                                paramMap[i] = JsonConvert.DeserializeObject(parameter[i].ToString(), type);
                            }

                             
                        }
                        catch (Exception e)
                        {
                            throw (new DirectParameterException("Illegal Argument: There did an Exception Occur while tryng to Desirialze the parameter " +
                                parmInfo[i].Name +" of the type "+ parmInfo[i].ParameterType.ToString() + " from json: "+parameter[i].ToString(),directRequest));
                        }
                    
                }
            }
            return paramMap;
        }

        private Object[] ResolveParametersByName(DirectRequest directRequest)
        {
            JArray parameter = ((JArray)directRequest.Data);
            return ResolveParametersByName(directRequest, parameter);
        }

        private Object[] ResolveParametersByName(DirectRequest directRequest, JArray parameter) 
        {
            ParameterInfo[] parmInfo = this.Method.GetParameters();
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
                                        throw (new DirectParameterException("Illegal Argument: There did an Exception Occur while tryng to Desirialze the parameter " +
                                            parmInfo[i2].Name + " of the type " + parmInfo[i2].ParameterType.ToString() + " from json: " + parameter[i2].ToString(), directRequest));
                                    }
                                }
                            }
                            i2++;
                        }
                    }
                }
            }
            return paramMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directRequest"></param>
        /// <returns></returns>
        private Object[] ResolveUpdateParameter(DirectRequest directRequest)
        {
            ParameterInfo[] parmInfo = this.Method.GetParameters();
            Object[] paramMap = new object[parmInfo.Length];

            JArray parameter = ((JArray)directRequest.Data);
            if (parameter != null)
            {
                if (parameter.Count <2)
                {
                    throw new Exception("Tried to call an Updatemethod with less than 2 Parameters.");
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
                        throw (new DirectParameterException("Illegal Argument: There did an Exception Occur while tryng to Desirialze the parameter " +
                            parmInfo[0].Name + " of the type " + parmInfo[0].ParameterType.ToString() + " from json: " + parameter[0].ToString(), directRequest));
                    }

                }

                JArray tempParams = new JArray();

                tempParams.Add(parameter[1]);

                paramMap = ResolveParametersByName(directRequest, tempParams);

                paramMap[0] = id;

            }
            return paramMap;

        }

        /// <summary>
        /// Calls this Direct Methods and parses the Paramter as via the customAttribute DirectMethodAttribute 
        /// configured
        /// </summary>
        /// <param name="parameter">An Object of Parametr</param>
        /// <returns></returns>
        internal Object invoke(DirectRequest directRequest, HttpSessionState sessionObject)
        {

            ParameterInfo[] parmInfo = this.Method.GetParameters();

            // will contain the paramters the function gets called with
            Object[] paramMap = new object[parmInfo.Length];


            if (directRequest.HttpRequest != null)
            {
                // do the methode wants to take care of the request by it self?
                if (parmInfo[0].ParameterType == directRequest.HttpRequest.GetType())
                {
                    paramMap[0] = directRequest.HttpRequest;
                }
                else
                {
                    HttpRequest curParam = directRequest.HttpRequest;
                    int i2 = 0;
                    // try to find Parameters in the Formvariables
                    foreach (var parm in parmInfo)
                    {
                        Type type = parmInfo[i2].ParameterType;
                        if (type.Name == "HttpPostedFile")
                        {

                            i2++;
                            continue;
                        }
                        String curentParameter = ((HttpRequest)curParam).Form[parm.Name];
                        if (curentParameter == null)
                        {

                            i2++;
                            continue;
                        }
                        try
                        {
                            if (type == System.Type.GetType("System.Object") || type == System.Type.GetType("System.String"))
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
                            throw (new DirectParameterException("Illegal Argument: There did an Exception Occur while tryng to Desirialze the parameter " +
                                parmInfo[i2].Name + " of the type " + parmInfo[i2].ParameterType.ToString() + " from json: " + curentParameter.ToString(), directRequest));
                        }

                        i2++;
                    }

                    i2 = 0;
                    // try to find Parameters in the Files-list
                    foreach (var parm in parmInfo)
                    {

                        Type type = parmInfo[i2].ParameterType;
                        if (((HttpRequest)curParam).Files[parm.Name] != null)
                        {
                            if (type.Name != "HttpPostedFile")
                            {
                                throw (new DirectParameterException("Illegal Argument: The Parameter "+parm.Name+" is not an instance of \"System.WebHttpPosted\" File.", directRequest));
                            }
                            paramMap[i2] = ((HttpRequest)curParam).Files[parm.Name];
                        }
                        i2++;
                    }
                }
                
                
            } else 
            if (this.MethodAttributes.ParameterHandling == ParameterHandling.PassThrough)
            {
                paramMap = ResolveParametersByIndex(directRequest);

            }
            else
            {
                switch (this.MethodAttributes.MethodType)
                {
                    case DirectMethodType.Normal:
                    case DirectMethodType.Create:
                    case DirectMethodType.Read:
                    case DirectMethodType.Delete:
                    case DirectMethodType.Hybrid:
                        paramMap = ResolveParametersByName(directRequest);
                        break;
                    case DirectMethodType.Update:
                        paramMap = ResolveUpdateParameter(directRequest);
                        break;
                    
                }
            }
            



    


            // will contain the paramters the function gets called with


          
            // this is not really usefull since the deserialization has allready been done...
            // maybe for forms?
           // if(da.SerializeParameterTypes){
                // serialize the attributes before invoking the method
            //}

            Type actionClassType = this.ParentAction.Type;
            Object actionInstanz = actionClassType.Assembly.CreateInstance(actionClassType.FullName);
            if(actionInstanz is IActionWithSessionState) {
                ((IActionWithSessionState)actionInstanz).SetSession(sessionObject);
            }
            return this.Method.Invoke(actionInstanz, paramMap);
        }

        /// <summary>
        /// Write API JSON.
        /// </summary>
        /// <param name="jw">The JSON writer.</param>
        internal void Write(JsonTextWriter jw)
        {
            jw.WriteStartObject();
            Utility.WriteProperty<string>(jw, "name", this.Name);
            Utility.WriteProperty<int>(jw, "len", this.Parameters);
            Utility.WriteProperty<bool>(jw, "formHandler", this.IsForm);
            jw.WriteEndObject();
        }

        /// <summary>
        /// Checks whether the passed method is a direct method.
        /// </summary>
        /// <param name="mi">The method to check.</param>
        /// <returns>True if the method is a direct method.</returns>
        internal static bool IsMethod(MethodInfo mi)
        {
            return Utility.HasAttribute(mi, typeof(DirectMethodAttribute));
        }

    }
}
