using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ExtDirect4DotNet.helper;
using log4net;
using Newtonsoft.Json;

namespace ExtDirect4DotNet {
    //TODO:Need to do a full review of this class and remove unused code. Also, this class could probably be made internal or even private within the DirectProxy class.
    public class DirectProvider {
        public const string REMOTING_PROVIDER = "remoting";

        private static readonly ILog Logger = LogManager.GetLogger(typeof (DirectProvider));

        private readonly Dictionary<string, DirectAction> _actions = new Dictionary<string, DirectAction>();

        private string _api = string.Empty;

        /// <summary>
        /// Creates an instance of the object.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="url">The url of the provider.</param>
        public DirectProvider(string name, string url) : this(name, url, REMOTING_PROVIDER) {
        }

        /// <summary>
        /// Creates an instance of the object.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="url">The url of the provider.</param>
        /// <param name="type">The type of the provider.</param>
        public DirectProvider(string name, string url, string type) {
            Name = name;
            Url = url;
            Type = type;
        }

        /// <summary>
        /// Indicates whether the provider has been configured or not.
        /// </summary>
        public bool IsConfigured { get; private set; }

        /// <summary>
        /// Gets/sets the name of the provider.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the type of the provider.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets/sets the url to router requests to for this provider.
        /// </summary>
        public string Url { get; set; }

        public override string ToString() {
            if (IsConfigured && string.IsNullOrEmpty(_api)) {
                using (StringWriter sw = new StringWriter()) {
                    using (JsonTextWriter jw = new JsonTextWriter(sw)) {
                        jw.WriteStartObject();
                        Utility.WriteProperty(jw, "type", Type);

                        Utility.WriteProperty(jw, "url", Url);
                        jw.WritePropertyName("actions");
                        jw.WriteStartObject();
                        foreach (DirectAction action in _actions.Values) {
                            action.Write(jw);
                        }
                        jw.WriteEndObject();
                        jw.WriteEndObject();
                        _api = string.Format("{0} = {1};", Name, sw.ToString());
                    }
                }
            }
            return _api;
        }

        /// <summary>
        /// Clears any previous configuration for this provider.
        /// </summary>
        public void Clear() {
            //NOTE:Not currently used.
            IsConfigured = false;
            _actions.Clear();
        }

        /// <summary>
        /// Configure the provider by adding the available API methods.
        /// </summary>
        /// <param name="assembly">The assembly to automatically generate parameters from.</param>
        public void Configure(Assembly assembly) {
            //NOTE:Not currently used.
            if (!IsConfigured) {
                List<Type> types = assembly.GetTypes().ToList();
                Configure(types);
            }
        }

        /// <summary>
        /// Configure the provider by adding the available API methods.
        /// </summary>
        /// <param name="assemblyList">A series of object instances that contain Ext.Direct methods.</param>
        public void Configure(Assembly[] assemblyList) {
            if (!IsConfigured) {
                List<Type> types = assemblyList.SelectMany(curAssembly => curAssembly.GetTypes()).ToList();
                Configure(types);
            }
        }

        /// <summary>
        /// Configure the provider by adding the available API methods.
        /// </summary>
        /// <param name="items">A series of object instances that contain Ext.Direct methods.</param>
        public void Configure(IEnumerable<object> items) {
            //NOTE:Not currently used.
            if (!IsConfigured) {
                List<Type> types = (items.Where(item => item != null).Select(item => item.GetType())).ToList();
                Configure(types);
            }
        }

        public void Configure(IEnumerable<Type> types) {
            foreach (Type type in types) {
                if (IsAction(type)) {
                    _actions.Add(type.Name, new DirectAction(type));
                }
            }
            IsConfigured = true;
        }

        /*internal object Execute(DirectRequest request) {
            //NOTE:Not currently used.
            DirectAction action = _actions[request.Action];
            if (action == null) {
                throw new DirectException(string.Format("Unable to find action, {0}", request.Action));
            }
            DirectMethod method = action.GetMethod(request.Method);
            if (method == null) {
                throw new DirectException(string.Format("Unable to find method, {0} in Action: {1}", request.Method, request.Action));
            }
            Type type = action.ActionType;
            return ""; //; method.Method.Invoke(type.Assembly.CreateInstance(type.FullName), request.Data);
        }*/

        /// <summary>
        /// Finds the action in the assemblies.
        /// </summary>
        /// <param name="request">the request you want to find the assembly to</param>
        /// <returns></returns>
        internal DirectAction GetDirectAction(string directActionName) {
            DirectAction action = _actions[directActionName];
            if (action == null) {
                DirectException exception = new DirectException(string.Format("Unable to find DirectAction: \"{0}\"", directActionName));
                Logger.Error(exception.Message, exception);
                throw exception;
            }
            return action;
        }

        internal DirectMethod GetDirectMethod(string directActionName, string directMethodName) {
            DirectAction action = GetDirectAction(directActionName);
            DirectMethod method = action.GetMethod(directMethodName);
            if (method == null) {
                DirectException exception = new DirectException(string.Format("Unable to find DirectMethod: \"{0}\" in DirectAction: \"{1}\"", directMethodName, directActionName));
                Logger.Error(exception.Message, exception);
                throw exception;
            }
            return method;
        }

        /// <summary>
        /// Checks whether a particular type is an Ext.Direct action.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is an Ext.Direct action.</returns>
        internal static bool IsAction(Type type) {
            return Utility.HasAttribute(type, typeof (DirectActionAttribute));
        }
    }
}