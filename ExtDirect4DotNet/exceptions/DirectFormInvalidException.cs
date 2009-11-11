using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExtDirect4DotNet.exceptions
{
    /// <summary>
    /// A spezialized Exception for ParameterException in FormSubmit
    /// This overrides the type of the Error from "exception" to "rpc" so an Form can mark its fields invalid...
    /// </summary>
    public class DirectFormInvalidException : DirectParameterException
    {
        
        /*public DirectFormInvalidException(DirectRequest directRequest, string fieldName, string message)
        {
        }*/
           

        public DirectFormInvalidException(DirectRequest directRequest, ArrayList errors) 
            : base(directRequest, errors)
        {
            base.type = "rpc";
        }

        public DirectFormInvalidException(Hashtable errors)
            : base(errors) 
        {
            base.type = "rpc";
        }
    }
}
