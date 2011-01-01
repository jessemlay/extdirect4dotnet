using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using ExtDirect4DotNet.helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExtDirect4DotNet {
    //TODO:Need to do a full review of this class and remove unused code.
    [DebuggerDisplay("Name = {Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    internal class DirectMethod {
        internal DirectMethod(MethodInfo method, DirectMethodType methType, DirectAction parentAction)
            : this(method, methType, parentAction, method.Name) {
        }

        internal DirectMethod(MethodInfo method, DirectMethodType methType, DirectAction parentAction, string directMethodName) {
            ParentAction = parentAction;
            MethodType = methType;
            Method = method;
            IsForm = (methType == DirectMethodType.Form); // FIX Utility.HasAttribute(method, typeof(DirectMethodFormAttribute));
            Name = directMethodName;
            MethodAttributes = ((DirectMethodAttribute) Method.GetCustomAttributes(typeof (DirectMethodAttribute), true)[0]);
        }

        /// <summary>
        /// Gets whether the method is a form method;
        /// </summary>
        [JsonProperty("formHandler")]
        internal bool IsForm { get; private set; }

        /// <summary>
        /// Gets the method info.
        /// </summary>
        internal MethodInfo Method { get; private set; }

        internal DirectMethodAttribute MethodAttributes { get; set; }

        internal DirectMethodType MethodType { get; private set; }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        [JsonProperty("name")]
        internal string Name { get; private set; }

        internal OutputHandling OutputHandling {
            get {
                DirectMethodAttribute da = ((DirectMethodAttribute) Method.GetCustomAttributes(typeof (DirectMethodAttribute), true)[0]);
                return da.OutputHandling;
            }
        }

        /// <summary>
        /// Gets the number of parameters for the method.
        /// </summary>
        [JsonProperty("len")]
        internal int Parameters {
            get {
                if (MethodAttributes.ParameterHandling == ParameterHandling.PassThrough && MethodType == DirectMethodType.Normal) {
                    return Method.GetParameters().Length;
                }

                if (MethodAttributes.ParameterHandling == ParameterHandling.AutoResolve) {
                    switch (MethodType) {
                        case DirectMethodType.Normal:
                            return 1;
                    }
                }

                switch (MethodType) {
                    case DirectMethodType.Create:
                    case DirectMethodType.Read:
                    case DirectMethodType.Delete:
                    case DirectMethodType.Form:
                        return 1;
                    case DirectMethodType.Update:
                    case DirectMethodType.TreeLoad:
                        return 2;
                }

                return Method.GetParameters().Length;
            }
        }

        internal DirectAction ParentAction { get; private set; }

        /// <summary>
        /// Invokes the specified method of the <see cref="DirectRequest"/>.
        /// Calls this Direct Methods and parses the parameter as via the customAttribute DirectMethodAttribute configured.
        /// </summary>
        /// <param name="directRequest">The direct request.</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        internal Object Invoke(DirectRequest directRequest, HttpContext httpContext) {
            HttpRequest httpRequest = directRequest.HttpRequest;

            ParameterInfo[] parmInfo = Method.GetParameters();

            // will contain the parameters the function gets called with
            Object[] paramMap = new object[parmInfo.Length];

            if (httpRequest != null) {
                // do the method wants to take care of the request by it self?
                if (parmInfo[0].ParameterType == httpRequest.GetType()) {
                    paramMap[0] = httpRequest;
                }
                else {
                    int i2 = 0;
                    // try to find Parameters in the Form variables
                    foreach (ParameterInfo parm in parmInfo) {
                        Type type = parmInfo[i2].ParameterType;
                        if (type.Name == "HttpPostedFile") {
                            i2++;
                            continue;
                        }
                        String curentParameter = (httpRequest).Form[parm.Name];
                        if (curentParameter == null) {
                            i2++;
                            continue;
                        }
                        try {
                            if (type == Type.GetType("System.Object") || type == Type.GetType("System.String")) {
                                paramMap[i2] = curentParameter;
                            }
                            else {
                                paramMap[i2] = JsonConvert.DeserializeObject(curentParameter, type);
                            }
                        }
                        catch (Exception ex) {
                            string message = string.Format("An Exception occurred while trying to deserialize the parameter {0} of the type {1} from json: {2}",
                                                           parmInfo[i2].Name, parmInfo[i2].ParameterType, curentParameter);
                            throw new DirectParameterException(message, ex, directRequest);
                        }

                        i2++;
                    }

                    int i3 = 0;
                    // try to find Parameters in the Files-list
                    foreach (ParameterInfo parm in parmInfo) {
                        Type type = parmInfo[i3].ParameterType;
                        if ((httpRequest).Files[parm.Name] != null) {
                            if (type.Name != "HttpPostedFile") {
                                throw new DirectParameterException(string.Format("The Parameter {0} is not an instance of \"System.WebHttpPosted\" File.", parm.Name), directRequest);
                            }
                            paramMap[i3] = (httpRequest).Files[parm.Name];
                        }
                        i3++;
                    }
                }
            }
            else if (MethodAttributes.ParameterHandling == ParameterHandling.PassThrough) {
                paramMap = ResolveParametersByIndex(directRequest);
            }
            else {
                switch (MethodAttributes.MethodType) {
                    case DirectMethodType.Normal:
                    case DirectMethodType.Create:
                    case DirectMethodType.Read:
                    case DirectMethodType.Delete:
                    case DirectMethodType.Hybrid:
                        paramMap = ResolveParametersByName(directRequest);
                        break;
                    case DirectMethodType.Update:
                    case DirectMethodType.TreeLoad: // use same function cause structure is similar
                        paramMap = ResolveUpdateParameter(directRequest);
                        break;
                }
            }

            // will contain the parameters the function gets called with

            // this is not really useful since the deserialization has already been done...
            // maybe for forms?
            // if(da.SerializeParameterTypes){
            // serialize the attributes before invoking the method
            //}

            Type actionClassType = ParentAction.ActionType;
            object actionInstanz = actionClassType.Assembly.CreateInstance(actionClassType.FullName);

            if (actionInstanz is IDirectAction) {
                ((IDirectAction) actionInstanz).CurrentHttpContext = httpContext;
            }
            
            object result = Method.Invoke(actionInstanz, paramMap);
            return result;
        }

        /// <summary>
        /// Write API JSON.
        /// </summary>
        /// <param name="jw">The JSON writer.</param>
        [Obsolete("This method needs to be removed and make use of JSON serialization with attributes.")]
        internal void Write(JsonTextWriter jw) {
            jw.WriteStartObject();
            Utility.WriteProperty(jw, "name", Name);
            Utility.WriteProperty(jw, "len", Parameters);
            Utility.WriteProperty(jw, "formHandler", IsForm);
            jw.WriteEndObject();
        }

        private Object[] ResolveParametersByIndex(DirectRequest directRequest) {
            ParameterInfo[] parmInfo = Method.GetParameters();
            Object[] paramMap = new object[parmInfo.Length];

            if (!(directRequest.Data is JValue)) {
                JArray parameter = ((JArray) directRequest.Data);
                int i = 0;
                // use the Position as Indicator
                for (i = 0; i < parameter.Count && i < paramMap.Length; i++) {
                    Type type = parmInfo[i].ParameterType;

                    try {
                        if (type == Type.GetType("System.Object")) {
                            paramMap[i] = parameter[i];
                        }
                        else {
                            paramMap[i] = JsonConvert.DeserializeObject(parameter[i].ToString(), type);
                        }
                    }
                    catch (Exception ex) {
                        string message = string.Format("An Exception occurred while trying to deserialize the parameter {0} of the type {1} from json: {2}",
                                                       parmInfo[i].Name, parmInfo[i].ParameterType, parameter[i]);
                        throw new DirectParameterException(message, ex, directRequest);
                    }
                }
            }
            return paramMap;
        }

        private Object[] ResolveParametersByName(DirectRequest directRequest) {
            //Future:Ext Direct will support named arguments in the future and therefore an object literal here.
            JArray parameter = ((JArray) directRequest.Data);
            return ResolveParametersByName(directRequest, parameter);
        }

        private Object[] ResolveParametersByName(DirectRequest directRequest, JArray parameter) {
            ParameterInfo[] parmInfo = Method.GetParameters();
            Object[] paramMap = new object[parmInfo.Length];

            if (parameter != null) {
                for (int i = 0; i < parameter.Count; i++) {
                    Object curParam = parameter[i];
                    if (curParam is JObject) {
                        int i2 = 0;

                        foreach (ParameterInfo parm in parmInfo) {
                            if (((JObject) curParam)[parm.Name] != null) {
                                Object curentParameter = ((JObject) curParam)[parm.Name];

                                if (curentParameter != null) {
                                    Type type = parmInfo[i2].ParameterType;
                                    try {
                                        if (type == Type.GetType("System.Object")) {
                                            paramMap[i2] = curentParameter;
                                        }
                                        else {
                                            paramMap[i2] = JsonConvert.DeserializeObject(curentParameter.ToString(), type);
                                        }
                                    }
                                    catch (Exception ex) {
                                        string message = string.Format("An Exception occurred while trying to deserialize the parameter {0} of the type {1} from json: {2}",
                                                                       parmInfo[i2].Name, parmInfo[i2].ParameterType, parameter[i2]);
                                        throw new DirectParameterException(message, ex, directRequest);
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
        /// Since a Update Function has another parameter Structure we need our own resolve logic function here 
        /// </summary>
        /// <param name="directRequest"></param>
        /// <returns></returns>
        private Object[] ResolveUpdateParameter(DirectRequest directRequest) {
            ParameterInfo[] parmInfo = Method.GetParameters();
            Object[] paramMap = new object[parmInfo.Length];

            //Future:Ext Direct will support named arguments in the future and therefore an object literal here.
            JArray parameter = ((JArray) directRequest.Data);
            if (parameter != null) {
                if (parameter.Count < 2) {
                    throw new DirectParameterException("Tried to call an Update method with less than 2 Parameters.", directRequest);
                }
                Type paramTyp = parmInfo[0].ParameterType;

                Object id = new Object();
                //paramMap[0] = ((paramTyp)parameter[0]);
                if (parameter[0] is JValue) {
                    try {
                        //TODO:Would this if expression ever resolve to false?  I don't think so.
                        if (paramTyp == Type.GetType("System.Object")) {
                            id = parameter[0];
                        }
                        else {
                            id = JsonConvert.DeserializeObject(parameter[0].ToString(), paramTyp);
                        }
                    }
                    catch (Exception ex) {
                        string message = string.Format("An Exception occurred while trying to deserialize the parameter {0} of the type {1} from json: {2}",
                                                       parmInfo[0].Name, parmInfo[0].ParameterType, parameter[0]);
                        throw new DirectParameterException(message, ex, directRequest);
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
        /// Checks whether the passed method is a direct method.
        /// </summary>
        /// <param name="mi">The method to check.</param>
        /// <returns>True if the method is a direct method.</returns>
        internal static bool IsMethod(MethodInfo mi) {
            return Utility.HasAttribute(mi, typeof (DirectMethodAttribute));
        }
    }
}