using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExtDirect4DotNet.exceptions
{

    /// <summary>
    /// Thrown when an error occurs during a Ext.Direct call.
    /// </summary>
    public class DirectException : ApplicationException
    {
        public string ExcetionType = "DirectException";

        public string type = "exception";

        public Hashtable result;

        public DirectRequest directRequest;

        public DirectException()
            : base()
        {
        }

        public DirectException(string msg)
            : base(msg)
        {
            result = new Hashtable();
            result.Add("success", false);
        }


        public DirectException(string msg, DirectRequest directRequest)
            : base(msg+" DirectAction: "+ directRequest.Action + " DirectMethod "+ directRequest.Method )
        {
        }

        public uint errorCode;

    }

    
}
