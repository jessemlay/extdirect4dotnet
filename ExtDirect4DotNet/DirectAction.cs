using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using ExtDirect4DotNet.helper;
using System.Xml;

namespace ExtDirect4DotNet
{
    /// <summary>
    /// This class represents a container for a series of related Ext.Direct methods.
    /// </summary>
    internal class DirectAction
    {

        private Dictionary<string, DirectMethod> methods;

        /// <summary>
        /// Creates an instance of this object.
        /// </summary>
        /// <param name="type">The type of the object.</param>
        internal DirectAction(Type type)
        {
            this.Type = type;
            this.methods = new Dictionary<string, DirectMethod>();
            this.Configure();
        }

        /// <summary>
        /// Gets the type of the action.
        /// </summary>
        internal Type Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        internal string Name
        {
            get
            {
                return this.Type.Name;
            }
        }

        /// <summary>
        /// Checks whether a particular type is an Ext.Direct action.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is an Ext.Direct action.</returns>
        internal static bool IsAction(Type type)
        {
            return Utility.HasAttribute(type, typeof(DirectActionAttribute));
        }

        /// <summary>
        /// Write API JSON.
        /// </summary>
        /// <param name="jw">The JSON writer.</param>
        internal void Write(JsonTextWriter jw)
        {
            jw.WritePropertyName(this.Name);
            jw.WriteStartArray();
            foreach (DirectMethod method in this.methods.Values)
            {
                method.Write(jw);
            }
            jw.WriteEndArray();
        }

        public string toDocString(XmlDocument docDesciptionPath)
        {
            string docString = "\n/**";
            foreach (XmlNode member in docDesciptionPath.GetElementsByTagName("member"))
            {
                if (member.Attributes["name"].Value.EndsWith(this.Name))
                {
                    docString += "\n * "+ member.FirstChild.InnerText;
                }
            }
            docString += " */ ";
            docString += "\nvar " + this.Name + " = {";

            int i = 0;
            foreach (DirectMethod method in this.methods.Values)
            {
                i++;

                docString += method.toDocString(docDesciptionPath) + ((i < this.methods.Values.Count) ? "," : "");
            }

            docString += "};";
            return docString;
        }

        internal void WriteDocumentation(JsonTextWriter jw)
        {
            jw.WriteRaw("var " + this.Name + " = ");
            jw.WriteStartObject();
            foreach (DirectMethod method in this.methods.Values)
            {
                method.Write(jw);
            }
            jw.WriteEndObject();
        }

        private void Configure()
        {
            foreach (MethodInfo mi in this.Type.GetMethods())
            {
                if (DirectMethod.IsMethod(mi))
                {
                    DirectMethodType methType = ((DirectMethodAttribute)mi.GetCustomAttributes(typeof(DirectMethodAttribute), true)[0]).MethodType;
                    if (methType == DirectMethodType.Hybrid)
                    {
                        this.methods.Add(mi.Name, new DirectMethod(mi, DirectMethodType.Normal, this));
                        this.methods.Add(mi.Name + "_Form", new DirectMethod(mi, DirectMethodType.Form, this, mi.Name + "_Form"));
                    }
                    else
                    {
                        this.methods.Add(mi.Name, new DirectMethod(mi, methType, this));
                    }
                }
            }
        }

        internal DirectMethod GetMethod(string name)
        {
            return this.methods[name];
        }

    }
}
