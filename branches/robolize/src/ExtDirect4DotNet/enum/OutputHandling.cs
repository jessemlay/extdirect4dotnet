using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtDirect4DotNet {
    /// <summary>
    /// Enumeration to specify the output processing of the <see cref="DirectMethod"/>.
    /// TODO:Need to review the functionality of each enumeration to validate it's fully implemented and functional.
    /// </summary>
    public enum OutputHandling {
        /// <summary>
        /// Used when the Method returns a Normal Object and this should get Serialized before it gets send to the Browser
        /// </summary>
        Normal,

        /// <summary>
        /// Signalize the router logic not do Serialisation after execution the method. (Defaults to false ) <br/>
        /// 
        /// If you return a json-string and do not set this to true you may end up with this: <br/>
        /// "... data[{\"propertiename\":\"propertyValue\"}] ..."  instead of <br/>
        /// "... date[{"propertiename":"propertieValue"}"] ... 
        /// </summary>
        JSON,

        /// <summary>
        /// Signalize the router logiic to handle the response of this methode as an collection of events
        /// The Returned Value has to be a <see cref="List{T}"/>.
        /// </summary>
        Poll
    }
}