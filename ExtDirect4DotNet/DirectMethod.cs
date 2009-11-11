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
using System.Xml;
using ExtDirect4DotNet.baseclasses;
using ExtDirect4DotNet.parametermapper;
using System.Collections;
using ExtDirect4DotNet.interfaces;

namespace ExtDirect4DotNet
{
    /// <summary>
    /// An Instance of this class Stores all the Information about a single Method
    /// thats Accessible via Extdirect. 
    /// Its been used by the Direct Proxy to get Information
    /// like the Parameter lenght, Methodname and ParentAction
    /// 
    /// And its been used Direct Processor to invoke the Method.
    /// 
    /// In this Class all the Parameter mapping logic happens
    /// </summary>
    internal class DirectMethod
    {


        internal DirectMethod(MethodInfo method, DirectMethodType methType, DirectAction parentAction)
            : this( method,  methType,  parentAction, method.Name)
        { }

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

        /// <summary>
        /// Contains all the Method Attribute this method is tagged with
        /// </summary>
        internal DirectMethodAttribute MethodAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the class this Method belongs to
        /// </summary>
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
        /// This returned Number may depends on the DirectMethodType and on the ParameterHandling
        /// Which can be set via Method attributes
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
                    case DirectMethodType.Read:
                    case DirectMethodType.Delete:
                    case DirectMethodType.Form:
                    case DirectMethodType.Update:
                         return 1;
                    case DirectMethodType.TreeLoad:
                        return 2;
                   
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

  

        /// <summary>
        /// Calls this Direct Methods and parses the Paramter as via the customAttribute DirectMethodAttribute 
        /// configured provided by the directRequest parameter
        /// </summary>
        /// <param name="directRequest">The Direct Request Object containing all information needed for the directMethodCall</param>
        /// <param name="instances">A Hashtable containing all DirectAction Class instances of the current Request for reuse</param>
        /// <returns>the Object Returned by the called function</returns>
        internal Object invoke(DirectRequest directRequest, Hashtable instances)
        {
            Type actionClassType = this.ParentAction.Type;

            // now check which interfacecs this class implments
            Boolean implementsIActionWithAfterCreation = false;
            Boolean implementsIActionWithBeforeInvoke = false;
            Boolean implementsIActionWithAfterInvoke = false;
            Boolean implementsICRUDAction = false;


            JArray requestData = directRequest.Data != null && ((JToken)directRequest.Data).HasValues != false ? ((JArray)directRequest.Data) : null;

            // create an instance of the class
            Object actionInstance = actionClassType.Assembly.CreateInstance(actionClassType.FullName);
            Boolean instanceJustCreated = false;

            string className = actionClassType.FullName;

            if (actionClassType is IActionWithMultipleInstancesPerRequest && requestData != null)
            {
                className = className + "-" + ((IActionWithMultipleInstancesPerRequest)actionInstance).getIstanceId((Hashtable)JsonConvert.DeserializeObject(requestData[0].ToString(), typeof(Hashtable)));
            }

            // check if during this request an instance of the current action the method belongs to
            // allready exists
            if (instances[className] == null)
            {
                // save the reference into instances
                instances[className] = actionInstance;
                instanceJustCreated = true;
            }
            else
            {
                actionInstance = instances[className];
            }

            foreach (var i in actionInstance.GetType().GetInterfaces())
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionWithAfterCreation<>))
                {
                    implementsIActionWithAfterCreation = true;
                } else 
                if(i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionWithBeforeInvoke<>)) {
                    implementsIActionWithBeforeInvoke = true;
                } else 
                if (i == typeof(IActionWithAfterInvoke))
                {
                    implementsIActionWithAfterInvoke = true;
                } else
                if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICRUDAction<>))
                {
                    implementsICRUDAction = true;
                }

            }

            // eventually call the afterCreation method
            if (instanceJustCreated && implementsIActionWithAfterCreation)
            {

                Type paramType = actionInstance.GetType().GetMethod("afterCreation").GetParameters()[0].ParameterType;


                // four differnet Possibiliteas 
                // 1. the afterCreation wants the DirectRequest
                if (paramType == typeof(DirectRequest))
                {
                    actionInstance.GetType().GetMethod("afterCreation").Invoke(actionInstance, new Object[] { directRequest });
                }
                else if (paramType == typeof(HttpContext))
                { // 2. The method asks for a HttpContext 
                    actionInstance.GetType().GetMethod("afterCreation").Invoke(actionInstance, new Object[] { directRequest.HttpContext });
                }
                else if (paramType == typeof(IList<>) && requestData != null)
                { // 3. The Action asks for a serializable type that implements IList so we will serialize all parametr into it
                    actionInstance.GetType().GetMethod("afterCreation").Invoke(actionInstance, new Object[] { JsonConvert.DeserializeObject(requestData.ToString(), paramType) });
                }
                else if (requestData != null)
                { // 4. the method wants a normal serialzed Object we will pass the first Parameter (usefull for CRUD actions)
                    actionInstance.GetType().GetMethod("afterCreation").Invoke(actionInstance, new Object[] { JsonConvert.DeserializeObject(requestData[0].ToString(), paramType) });
                }
                else
                { // A method with no parameter was called... ther is no data to serialize
                    actionInstance.GetType().GetMethod("afterCreation").Invoke(actionInstance, new Object[] { null });
                }

                // call the method setStoreParameter which exists for all implementation of ICRUDAction with the first parameter of the store request

            
            }
            if (implementsIActionWithBeforeInvoke)
            {
                // getting the type of the first paramter to deserialize to it
                Type paramType = actionInstance.GetType().GetMethod("beforeMethodInvoke").GetParameters()[1].ParameterType;

                // create the parameter object to call the method with
                Object[] parameter = new Object[2];
                parameter[0] = this.Name;
                // four differnet Possibiliteas 
                // 1. the beforeMethodInvoke wants the DirectRequest
                if (paramType == typeof(DirectRequest))
                {
                    parameter[1] = directRequest;
                }
                else if (paramType == typeof(HttpContext))
                { // 2. The method asks for a HttpContext 
                    parameter[1] = directRequest.HttpContext;
                }
                else if (paramType == typeof(IList<>) && requestData != null)
                { // 3. The Action asks for a serializable type that implements IList so we will serialize all parametr into it
                    parameter[1] = JsonConvert.DeserializeObject(requestData.ToString(), paramType);
                }
                else if (requestData != null)
                { // 4. the method wants a normal serialzed Object we will pass the first Parameter (usefull for CRUD actions)
                    parameter[1] = JsonConvert.DeserializeObject(requestData[0].ToString(), paramType);
                }
                else
                {
                    parameter[1] = null;
                }
                actionInstance.GetType().GetMethod("beforeMethodInvoke").Invoke(actionInstance, parameter);
            }

            // get the method info to mapp the parameter with
            ParameterInfo[] paramInfo = this.Method.GetParameters();
         
            Object[] paramMap = null;
            IMetaData metadata = null;
            string rootname = null;
            // just collect additional Information for CRUD
            if (implementsICRUDAction)
            {
                metadata = actionInstance.GetType().GetMethod("getMetaData").Invoke(actionInstance, new Object[] { }) as IMetaData;
                rootname = metadata.getRootPropertyName();
            }
            
            DirectMethodType methodType = this.MethodAttributes.MethodType;

            if (directRequest.IsForm)
            {
                FormParameterMapper formMapper = new FormParameterMapper();
                paramMap = formMapper.MapParameter(paramInfo, directRequest);
            }
            else if (methodType == DirectMethodType.Normal && this.MethodAttributes.ParameterHandling == ParameterHandling.PassThrough)
            {
                IndexParameterMapper indexMapper = new IndexParameterMapper();
                paramMap = indexMapper.MapParameter(paramInfo, directRequest);
            }
            else
            {
                NameParameterMapper nameMapper;
                switch (methodType)
                {
                    case DirectMethodType.Read:
                        // check if it implements ICRUD Action
                        if (!implementsICRUDAction)
                        {
                            throw new Exception("The Class " + this.ParentAction.Name + " has to implement the interface ICRUDAction if it wants to implement a CRUD Action. (Tried to call " + this.Name + " which is markt as a CRUD method");
                        }
                        nameMapper = new NameParameterMapper();
                        paramMap = nameMapper.MapParameter(paramInfo, directRequest);
                        break;
                    case DirectMethodType.Normal:
                    case DirectMethodType.Hybrid:
                        nameMapper = new NameParameterMapper();
                        paramMap = nameMapper.MapParameter(paramInfo, directRequest);
                        break;
                    case DirectMethodType.Update:
                    case DirectMethodType.Create:
                    case DirectMethodType.Delete:
                        // check if it implements ICRUD Action
                        if (!implementsICRUDAction)
                        {
                            throw new Exception("The Class " + this.ParentAction.Name + " has to implement the interface ICRUDAction if it wants to implement a CRUD Action. (Tried to call " + this.Name + " which is markt as a CRUD method");
                        }
                        CUDParameterMapper updateMaper1 = new CUDParameterMapper();
                        paramMap = updateMaper1.MapParameter(paramInfo, directRequest, rootname);
                        break;
                    case DirectMethodType.TreeLoad: // use same function cause structure is simmilar
                        CUDParameterMapper updateMaper = new CUDParameterMapper();
                        paramMap = updateMaper.MapParameter(paramInfo, directRequest);
                        break;
                    
                }
            }

            // actually inkoe the method save the reference to the returned value
            Object result = this.Method.Invoke(actionInstance, paramMap);

            

            // add post invoke handling for crud methods here
            if (methodType == DirectMethodType.Update || 
                methodType == DirectMethodType.Create || 
                methodType == DirectMethodType.Delete || 
                methodType == DirectMethodType.Read )
            {
                Hashtable resultWrapper = new Hashtable();

                

                if (methodType == DirectMethodType.Create)
                {
                    // add the orginal result to the hashtable
                    // okay Ext want it to be an Array in case of an Create... so wrap it eventually
                    if (methodType == DirectMethodType.Create && !result.GetType().IsArray && !(result is IList))
                    {
                        // okay lets wrap the result for extjs :)
                        result = new Object[] { result };
                    }
                }


                if (methodType == DirectMethodType.Update)
                {
                    // do nothing yet just return the returned value (maybe for batch this behave needs a change)
                }

                if (methodType == DirectMethodType.Delete)
                {
                    resultWrapper.Add(metadata.getSuccessPropertyName(), result);
                    result = resultWrapper;
                }

                
                
                if (methodType == DirectMethodType.Read )
                {
                    resultWrapper.Add(metadata.getSuccessPropertyName(), true);
                    if (((bool)actionInstance.GetType().GetMethod("addMetaData").Invoke(actionInstance, new Object[] { })))
                        resultWrapper.Add("metaData", metadata);
                    resultWrapper.Add(metadata.getTotalPropertyName(), ((int)actionInstance.GetType().GetMethod("getResultCount").Invoke(actionInstance, new Object[] { })));
                    resultWrapper.Add(metadata.getRootPropertyName(), result);

                    result = resultWrapper;
                }
            }

            // after invoke handling?
            if (implementsIActionWithAfterInvoke)
            {
                // call the afterMethodInvoke Method with the result
                ((IActionWithAfterInvoke)actionInstance).afterMethodInvoke(this.Name, result);
            }
            return result;
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

        private string resolveParamType(ParameterInfo paramInfo)
        {
            // TODO implement more than just string....
            /*
            switch (paramInfo.GetType().ToString())
            {
                case ((new String("")).GetType().ToString()):
                    return "String";
                


            }*/
            return "string";
        }

        private string getParameterDesciption(string paramName) {
            return paramName + " parameter Descirption";
        }

        public string toDocString(XmlDocument docDesciptionDoc)
        {
            if (this.MethodAttributes.ParameterHandling == ParameterHandling.PassThrough &&
                    this.MethodType == DirectMethodType.Normal)
            {
                string functionString = this.Method.Name + " : function (";
                string docString = "/**\n";
                docString += " * "+ "Method description " + "\n";
                foreach(ParameterInfo param in this.Method.GetParameters()) {
                    docString += " * @param {"+resolveParamType(param) + "} " + getParameterDesciption(param.Name) + "\n";
                    functionString += param.Name + ", ";
                }
                docString += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
                docString += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
                functionString += "cb, scope) {}";
                docString += " */ \n "+ functionString;
                return docString;
                
            }
            else if (this.MethodAttributes.ParameterHandling == ParameterHandling.AutoResolve)
            {
                switch (this.MethodType)
                {
                    case DirectMethodType.Normal:
                        string functionString = this.Method.Name + " : function (hash,";
                        string docString = "/**\n";
                        docString += " * "+ "Method description " + "\n";
                        docString += " * @param {Object} hash \n";
                        foreach(ParameterInfo param in this.Method.GetParameters()) {
                            docString += " * @param {"+resolveParamType(param) + "} hash." + getParameterDesciption(param.Name) + "\n";
                            //functionString += param.Name + ", ";
                        }
                        
                        docString += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
                        docString += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
                        functionString += "cb, scope) {}";
                        docString += " */ \n "+ functionString;
                        return docString;

                }
            } 
            switch (this.MethodType)
            {
                case DirectMethodType.Create:
                    string functionString = this.Method.Name + " : function (rec,";
                    string docString = "/**\n";
                    docString += " * "+ "Method description " + "\n";
                    docString += " * @param {Ext.data.Record} rec \n";
                    docString += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
                    docString += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
                    functionString += "cb, scope) {}";
                    docString += " */ \n "+ functionString;
                    return docString;
                case DirectMethodType.Read:
                     // Ein Parameter (Read parameter vom Store)
                    functionString = this.Method.Name + " : function (params,";
                    docString = "/**\n";
                    docString += " * "+ "Method description " + "\n";
                    docString += " * @param {Object} params parameter of the Store \n";
                    docString += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
                    docString += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
                    functionString += "cb, scope) {}";
                    docString += " */ \n "+ functionString;
                    return docString;
                case DirectMethodType.Delete:
                    functionString = this.Method.Name + " : function (id,";
                    docString = "/**\n";
                    docString += " * "+ "Method description " + "\n";
                    docString += " * @param id Id of the Record to delete \n";
                    docString += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
                    docString += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
                    functionString += "cb, scope) {}";
                    docString += " */ \n "+ functionString;
                    return docString;
                case DirectMethodType.Form:
                    functionString = this.Method.Name + " : function (form,";
                    docString = "/**\n";
                    docString += " * "+ "Method description " + "\n";
                    docString += " * @param form a dom form Node or an Ext.form.BasicForm \n";
                    docString += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
                    docString += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
                    functionString += "cb, scope) {}";
                    docString += " */ \n "+ functionString;
                    return docString;
                case DirectMethodType.Update: // Zwei Parameter Die Id des Records und der Record der geupdatet werden soll
                    functionString = this.Method.Name + " : function (id, record";
                    docString = "/**\n";
                    docString += " * "+ "Method description " + "\n";
                    docString += " * @param id Id of the Record to update \n";
                    docString += " * @param record {Object} the recordFields you wish to update \n";
                    docString += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
                    docString += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
                    functionString += "cb, scope) {}";
                    docString += " */ \n "+ functionString;
                    return docString;
                case DirectMethodType.TreeLoad: // Zwei Parameter die ID des Parents und die Parameter die die Funktion füllen sollen.
                    functionString = this.Method.Name + " : function (id, params";
                    docString = "\n/**\n";
                    docString += " * "+ "Method description " + "\n";
                    docString += " * @param id Id the Node to load \n";
                    docString += " * @param params extra Prameter you want to call the loadmethod with \n";
                    docString += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
                    docString += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
                    functionString += "cb, scope) {}";
                    docString += " */ \n "+ functionString;
                    return docString;

            }

            string functionString2 = this.Method.Name + " : function (";
            string docString2 = "/**\n";
            docString2 += " * " + "Method description " + "\n";
            foreach (ParameterInfo param in this.Method.GetParameters())
            {
                docString2 += " * @param {" + resolveParamType(param) + "} " + getParameterDesciption(param.Name) + "\n";
                functionString2 += param.Name + ", ";
            }
            docString2 += " * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) \n";
            docString2 += " * @param scope {Object} The Scope to call the Callbackfunction with. \n";
            functionString2 += "cb, scope) {}";
            docString2 += " */ \n " + functionString2;
            return docString2;

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
