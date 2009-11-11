using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExtDirect4DotNet.exceptions
{
    /// <summary>
    /// Implementation of an Parameter exception.
    /// It contains all known Information why the Exception ocurred
    /// 
    /// Notice this is a base Exception used for the store for example.
    /// 
    /// For Forms throw the FormDataInvalidException which type is not 'exception' to 
    /// let the form mark it self as invalid....
    /// </summary>
    public class DirectParameterException : DirectException
    {
        private ArrayList errors;
        private string msg;

        public DirectParameterException(string msg, DirectRequest directRequest, ArrayList errors)
            : base("")
        {
            this.directRequest = directRequest;
            this.result.Add("errors", errors);
        }

        public DirectParameterException(DirectRequest directRequest, ArrayList errors)
            : this("", directRequest, errors)
        {

        }

        public DirectParameterException(ArrayList errors) : base("")
        {
            this.result.Add("errors", errors);
        }


        public DirectParameterException(Hashtable errors)
            : base("")
        {
            ArrayList errorsList = new ArrayList();
            foreach (string key in errors.Keys)
            {
                Hashtable ht = new Hashtable();
                
                ht.Add("id", key);
                ht.Add("msg", errors[key]);
                errorsList.Add(ht);
            }
            this.result.Add("errors", errorsList);
        }


        
        public DirectParameterException(string msg, DirectRequest directRequest)
            : this(msg, directRequest, null)
        {

        }



        public override string Message
        {
            get
            {
                string msg = "";
                if (directRequest == null)
                    msg = "Parameter exception. An Exception occured while trying to call a Method. Direct request not set yet! "+this.msg;
                if (directRequest != null)
                    msg = "An error occures while trying to call the Method " + directRequest.Method + " of the Class " + directRequest.Action + ". One or more Parameter was invalid. Message: " + this.msg;
                return msg;
            }
        }
    }
}
