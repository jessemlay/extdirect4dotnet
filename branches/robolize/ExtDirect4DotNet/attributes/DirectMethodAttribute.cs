using System;
using System.Linq;

namespace ExtDirect4DotNet {
    public enum DirectMethodType {
        /// <summary>
        /// Contains a single Parameter with the Record to save.
        /// If the method has only One Paramter the Parametr gets deserialzed with the typ of that parameter.
        /// </summary>
        Create,

        /// <summary>
        /// Since The parameter will come as a Hash e.g.: {sort: 'name', sortDir: 'DESC'}
        /// the Hash will get mapted via Autoresolve.
        /// 
        /// The Function should have at least those two Parameters: (string sort, string sortDir).
        /// </summary>
        Read,

        /// <summary>
        /// Len gets set to 2 since the Update function gets called with two parameters
        /// First is the Id of the Record, Second is the record with the updated data.
        /// If The DirectMethod it self has two parametrs, the Seconde Parameter in the Request
        /// Gets Serialized with the same type than the Function.
        /// </summary>
        Update,

        /// <summary>
        /// The Function will have only one Parameter the Id.
        /// </summary>
        Delete,

        /// <summary>
        /// Len gets set tu 2 since the Treeloader will send two values to the Server, the Node Id and an object of Parameter
        /// </summary>
        TreeLoad,

        /// <summary>
        /// Simple Function...
        /// </summary>
        Normal,

        /// <summary>
        /// Function with Form Specifix Handling (len gets set to 1 and the parameter will read out of the httpContext.Form)
        /// </summary>
        Form,

        /// <summary>
        /// This Method gets Handled as a normal Method but the Proxy will generate another Method with the posfix "_form"
        /// where len gets set to 1 and the Parameterhandling for this gets done as a Form Method
        /// </summary>
        Hybrid
    }

    public enum ParameterHandling {
        /// <summary>
        /// No Special Handling will be done
        /// The Parameter mapping been done by parameter Position 
        /// </summary>
        PassThrough,

        /// <summary>
        /// Set this to true to set len propertie to for normal Methodes (No CRUD Methods special handling) to 1 and try to resolve the parameter.
        /// This makse it possible to submit paramers independend from their order same technic as in exct used 
        /// minimize paramter to one containing all the properties {name: 'Martin', lastname: 'Spaeth'} will execute 
        /// the following sample method
        /// 
        /// [DirectMethod(ResolveParameterNames=true)]
        /// public string sample(string name, string lastname){
        ///    return "{name:"+name+",lastname:"+lastname"}";
        /// }
        /// 
        /// will return an API definition like that:
        /// 
        /// "ActionName":[{"name":"sample","len":1,"formHandler":false}
        /// 
        /// And can get called from Ext like that:
        /// 
        /// ActionName.sample({lastname:'spaeth',name:'martin'});
        /// 
        /// </summary>
        AutoResolve
    }

    public enum OutputHandling {
        /// <summary>
        /// Used when the Method returns a Normal Object and this should get Serialized before it gets send to the Browser
        /// </summary>
        Normal,

        /// <summary>
        /// Signalize the router logic not do Serialisation after execution the method (Defaults to false )
        /// 
        /// If you return a json-string and do not set this to true you may end up with this:
        /// "... data[{\"propertiename\":\"propertyValue\"}] ..."  instead of
        /// "... date[{"propertiename":"propertieValue"}"] ... 
        /// </summary>
        JSON,

        /// <summary>
        /// Signalize the router logiic to handle the response of this methode as an collection of events
        /// 
        /// The Returned Value has to be a List<DirectEvent>
        /// </summary>
        Poll
    }

    /// <summary>
    /// This attribute should be added to methods that will be Ext.direct methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DirectMethodAttribute : Attribute {
        /// <summary>
        /// Set this Option to specifie this Method for the CRUD Actions
        /// </summary>
        public DirectMethodType MethodType = DirectMethodType.Normal;

        /// <summary>
        /// Set this Option for Specific output Handling
        /// </summary>
        public OutputHandling OutputHandling = OutputHandling.Normal;

        /// <summary>
        /// Set this Option for specific parameter-mapping handling
        /// This may influnce the len propertie of the Function
        /// </summary>
        public ParameterHandling ParameterHandling = ParameterHandling.PassThrough;

        /// <summary>
        /// Set this to true to let ExtDirect4DotNet try to SerializeParameter Types for you.
        /// 
        /// This Option makes is Usefull when you want to Call a method with a Date for Example
        /// The Router will use the Newtonsofts deserialize Function and trys to deserialize the Json string into 
        /// The needed parameter
        /// </summary>
        public bool SerializeParameterTypes;
    }
}