using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ExtDirect4DotNet {
    //TODO:Need to do a full review of this class and remove unused code.
    /// <summary>
    /// This class represents a container for a series of related Ext.Direct methods.
    /// </summary>
    [DebuggerDisplay("ActionName = {ActionName}")]
    [JsonObject(MemberSerialization.OptIn)]
    internal class DirectAction {
        private readonly Dictionary<string, DirectMethod> _methods = new Dictionary<string, DirectMethod>();

        //TODO:This property needs to be read as a JArray.
        internal Dictionary<string, DirectMethod> DirectMethods {
            get { return _methods; }
        }

        /// <summary>
        /// Creates an instance of this object.
        /// </summary>
        /// <param name="type">The type of the object.</param>
        internal DirectAction(Type type) {
            ActionType = type;
            LoadDirectMethods();
        }

        /// <summary>
        /// Gets the type of the action.
        /// </summary>
        internal Type ActionType { get; private set; }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        //[JsonProperty("dynamic")]
        //TODO:This JSON property needs to have a dynamic name.
        internal string ActionName {
            get { return ActionType.Name; }
        }

        internal DirectMethod GetMethod(string name) {
            return _methods[name];
        }

        /// <summary>
        /// Write API JSON.
        /// </summary>
        /// <param name="jw">The JSON writer.</param>
        [Obsolete("This method needs to be removed and make use of JSON serialization with attributes.")]
        internal void Write(JsonTextWriter jw) {
            jw.WritePropertyName(ActionName);
            jw.WriteStartArray();
            foreach (DirectMethod method in _methods.Values.OrderBy(m => m.Name)) {
                method.Write(jw);
            }
            jw.WriteEndArray();
        }

        private void LoadDirectMethods() {
            foreach (MethodInfo methodInfo in ActionType.GetMethods()) {
                if (!DirectMethod.IsMethod(methodInfo)) {
                    continue;
                }

                DirectMethodAttribute attribute = (DirectMethodAttribute) methodInfo.GetCustomAttributes(typeof (DirectMethodAttribute), true)[0];
                DirectMethodType methType = attribute.MethodType;
                if (methType == DirectMethodType.Hybrid) {
                    _methods.Add(methodInfo.Name, new DirectMethod(methodInfo, DirectMethodType.Normal, this));

                    string name = string.Format("{0}_Form", methodInfo.Name);
                    _methods.Add(name, new DirectMethod(methodInfo, DirectMethodType.Form, this, name));
                }
                else {
                    _methods.Add(methodInfo.Name, new DirectMethod(methodInfo, methType, this));
                }
            }

        }
    }
}