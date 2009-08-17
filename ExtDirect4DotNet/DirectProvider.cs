using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExtDirect4DotNet.helper;


namespace ExtDirect4DotNet
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectProvider
    {

        public const string REMOTING_PROVIDER = "remoting";

        private Dictionary<string, DirectAction> actions;
        private string api = string.Empty;
        private string doc = string.Empty;

        /// <summary>
        /// Creates an instance of the object.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="url">The url of the provider.</param>
        public DirectProvider(string name, string url)
            : this(name, url, REMOTING_PROVIDER)
        {
        }

        /// <summary>
        /// Creates an instance of the object.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="url">The url of the provider.</param>
        /// <param name="type">The type of the provider.</param>
        public DirectProvider(string name, string url, string type)
        {
            this.Name = name;
            this.Url = url;
            this.Type = type;

            actions = new Dictionary<string, DirectAction>();
        }

        /// <summary>
        /// Configure the provider by adding the available API methods.
        /// </summary>
        /// <param name="assembly">The assembly to automatically generate parameters from.</param>
        public void Configure(Assembly assembly)
        {
            if (!this.Configured)
            {
                List<Type> types = new List<Type>();
                foreach (Type type in assembly.GetTypes())
                {
                    types.Add(type);
                }
                this.Configure(types);
            }
        }


        /// <summary>
        /// Configure the provider by adding the available API methods.
        /// </summary>
        /// <param name="items">A series of object instances that contain Ext.Direct methods.</param>
        public void Configure(Assembly[] assemblyList)
        {
             
            if (!this.Configured)
            {
                List<Type> types = new List<Type>();
                foreach (var curAssembly in assemblyList)
                {
                    foreach (Type type in curAssembly.GetTypes())
                    {
                       
                        types.Add(type);
                    }                   
                }
               
                this.Configure(types);
            }
        }

        /// <summary>
        /// Configure the provider by adding the available API methods.
        /// </summary>
        /// <param name="items">A series of object instances that contain Ext.Direct methods.</param>
       /*
        public void Configure(Type[] typelist)
        {

            if (!this.Configured)
            {
                List<object> types = new List<object>();
                foreach (var allAssembly in assemblyList)
                {
                    if (allAssembly != null)
                    {
                        types.AddRange(allAssembly.GetTypes());
                    }
                }

                this.Configure(types);
            }
        }*/
        /// <summary>
        /// Configure the provider by adding the available API methods.
        /// </summary>
        /// <param name="items">A series of object instances that contain Ext.Direct methods.</param>
        public void Configure(IEnumerable<object> items)
        {
            if (!this.Configured)
            {
                List<Type> types = new List<Type>();
                foreach (object item in items)
                {
                    if (item != null)
                    {
                        types.Add(item.GetType());
                    }
                }
                this.Configure(types);
            }
        }

        //Generic configuration method for a list of types.
        private void Configure(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if(type.Name == "Class1"){
                    string test = "";
                }
                if (DirectAction.IsAction(type))
                {
                    this.actions.Add(type.Name, new DirectAction(type));
                }
            }
            this.Configured = true;
        }

        /// <summary>
        /// Clears any previous configuration for this provider.
        /// </summary>
        public void Clear()
        {
            this.Configured = false;
            this.actions.Clear();
        }

        /// <summary>
        /// Indicates whether the provider has been configured or not.
        /// </summary>
        public bool Configured
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets/sets the name of the provider.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets the url to router requests to for this provider.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets the type of the provider.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        public override string ToString()
        {
            if (this.Configured && String.IsNullOrEmpty(this.api))
            {
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (JsonTextWriter jw = new JsonTextWriter(sw))
                    {
                        jw.WriteStartObject();
                        Utility.WriteProperty<string>(jw, "type", this.Type);

                        Utility.WriteProperty<string>(jw, "id", "1");
                       // Utility.WriteProperty<int>(jw, "enableBuffer", 3000);
                        Utility.WriteProperty<string>(jw, "url", this.Url);
                        jw.WritePropertyName("actions");
                        jw.WriteStartObject();
                        foreach (DirectAction action in this.actions.Values)
                        {
                            action.Write(jw);
                        }
                        jw.WriteEndObject();
                        jw.WriteEndObject();
                        this.api = String.Format("{0} = {1};", this.Name, sw.ToString());
                    }
                }
            }
            return this.api;
        }

        public string toDocString()
        {
            this.doc = "";
            if (this.Configured && String.IsNullOrEmpty(this.doc))
            {
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (JsonTextWriter jw = new JsonTextWriter(sw))
                    {
                        /*
                        jw.WriteStartObject();
                        Utility.WriteProperty<string>(jw, "type", this.Type);

                        Utility.WriteProperty<string>(jw, "id", "1");
                         * 
                        // Utility.WriteProperty<int>(jw, "enableBuffer", 3000);
                        Utility.WriteProperty<string>(jw, "url", this.Url);
                        jw.WritePropertyName("actions");
                        jw.WriteStartObject();*/
                        foreach (DirectAction action in this.actions.Values)
                        {
                            this.doc += action.toDocString();
                        }
                        /*
                        jw.WriteEndObject();
                        jw.WriteEndObject();*/
                        this.api = String.Format("{0} = {1};", this.Name, sw.ToString());
                    }
                }
            }
            return this.doc;
        }

        /// <summary>
        /// Finds the action in the assemblys
        /// </summary>
        /// <param name="request">the request you want to find the assembly to</param>
        /// <returns></returns>
        internal DirectAction GetDirectAction(DirectRequest request)
        {
            DirectAction action = this.actions[request.Action];
            if (action == null)
            {
                throw new DirectException("Unable to find action, " + request.Action,request);
            }
            return action;
        }

        internal DirectMethod GetDirectMethod(DirectRequest request)
        {
            DirectAction action = GetDirectAction(request);
            DirectMethod method = action.GetMethod(request.Method);
            if (method == null)
            {
                throw new DirectException("Unable to find method, " + request.Method + " in Action: " + request.Action,request);
            }
            return method;
        }

        internal object Execute(DirectRequest request)
        {
            DirectAction action = this.actions[request.Action];
            if (action == null)
            {
                throw new DirectException("Unable to find action, " + request.Action,request);
            }
            DirectMethod method = action.GetMethod(request.Method);
            if (method == null)
            {
                throw new DirectException("Unable to find method, " + request.Method + " in Action: " +request.Action,request);
            }
            Type type = action.Type;
            return "";//; method.Method.Invoke(type.Assembly.CreateInstance(type.FullName), request.Data);
        }



    }
}
