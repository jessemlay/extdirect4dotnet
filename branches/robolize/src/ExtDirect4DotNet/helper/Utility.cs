using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ExtDirect4DotNet.helper {
    internal static class Utility {
        /// <summary>
        /// Checks whether a type has a particular attribute.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>True if the type contains the attribute.</returns>
        internal static bool HasAttribute(Type t, Type attribute) {
            try {
                return t.GetCustomAttributes(attribute, true).Length > 0;
            }
            catch {
                // FIX """
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks whether a method has a particular attribute.
        /// </summary>
        /// <param name="t">The method.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>True if the method contains the attribute.</returns>
        internal static bool HasAttribute(MethodInfo mi, Type attribute) {
            return mi.GetCustomAttributes(attribute, true).Length > 0;
        }

        /// <summary>
        /// Shortcut method to write a property value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="jw">The JSON writer.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        internal static void WriteProperty<T>(JsonTextWriter jw, string name, T value) {
            jw.WritePropertyName(name);
            jw.WriteValue(value);
        }
    }
}